using System; 
using System.Threading.Tasks;
using AutoMapper; 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.DTO;
using Shared.Authentication;
using ShoppingCart.DTO;
using Taxology.Site.Models;
using Taxology.Site.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Taxology.Site.Controllers
{
    [Authorize]
    public class PurchaseController : BaseWebController
    { 
        private readonly ICustomerRestService customerService;
        private readonly ILogger logger;
        private readonly IOrderRestService orderService; 
        private readonly IMapper mapper;
        private readonly IShoppingCartService shoppingCartService;

        private readonly string purchaseModelKey = "purchaseModel";

        public PurchaseController(ICustomerRestService customerService, ILoggerFactory loggerFactory
            , IOrderRestService orderService, IMapper mapper, IShoppingCartService shoppingCartService,
            UserManager<SiteUser> userManager)
        : base(userManager)
        { 
            this.customerService = customerService;
            this.logger = loggerFactory.CreateLogger<PurchaseController>();
            this.orderService = orderService; 
            this.mapper = mapper;
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index(Guid cartId)
        {
            try
            {
                var user = await GetSiteUser();
                var model = new PurchaseModel();
                model.CustomerId = user.CustomerId;
                model.CartId = cartId;

                var openOrder = await orderService.GetOpenOrderForCustomer(model.CustomerId);

                if (openOrder == null)
                {
                    model.Step = 1;

                    return View(model);
                }
                else
                {
                    model.Order = mapper.Map<OrderModel>(openOrder);
                    model.Billing = mapper.Map<BillingInfoModel>(openOrder.BillingAddress);

                    //TODO should update the order with any new products
                    switch (openOrder.OrderStatus)
                    {
                        case OrderStatus.Created:
                            model.Step = 2;
                            break;
                        case OrderStatus.Confirmed:
                            model.Step = 3;
                            break; 
                    }
                       return RouteToContinue(model);
//                    if (openOrder.OrderStatus == OrderStatus.Confirmed)
//                    {
//                        model.Step = 2;
//                    }
//                    else
//                    {
//                        model.Step = 3;
//                    }
//
//                    return RouteToContinue(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private void AddTempData(PurchaseModel model)
        {
            model.Billing.CountryList = null;
            model.Billing.StateList = null;
            string modelString = JsonConvert.SerializeObject(model);
            TempData[purchaseModelKey] = modelString;

        }

        private PurchaseModel GetTempData()
        {
            string modelString = TempData[purchaseModelKey] as string;
            if (!string.IsNullOrEmpty(modelString))
            {
                PurchaseModel tempModel = JsonConvert.DeserializeObject<PurchaseModel>(modelString);
                return tempModel;
            }

            return null;
        }

        public ActionResult Continue(PurchaseModel model)
        {
            try
            { 
                var tempModel = GetTempData(); 

                if (tempModel != null)
                { 
                    return View("Index", tempModel);
                } 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ordering");
            }

            return RedirectToAction("Unable");

        }

        public async Task<IActionResult> Unable()
        {
            return View();
        }
         
        [HttpPost]
        public async Task<IActionResult> SaveBilling(PurchaseModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                var user = await GetSiteUser();

                var cart = await shoppingCartService.GetShoppingCart();
             
                //TODO SAGA -- don't really need it here. worst case is that customer isn't updated
                model = await CreateOrder(model, user, cart);
                if (model.Order != null)
                { 
                   // wait till saga and separate data stores in place await SaveCustomer(model, user);
                }
             
                model.Step = 2;

                return RouteToContinue(model);
                // return View("Index", model); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ordering");
            }

            return RedirectToAction("Unable");
        }


        private IActionResult RouteToContinue(PurchaseModel model)
        {
            AddTempData(model);
            return RedirectToAction("Continue");
        }

        private async Task<PurchaseModel> CreateOrder(PurchaseModel model, SiteUser user, ShoppingCartDTO cart)
        {
            var customer = await customerService.GetCustomer(user.CustomerId);
            var billing = mapper.Map<BillingAddressDTO>(model.Billing);

            var order = await orderService.CreateOrder(customer, cart, billing);

            //on error return false;
            model.Order = mapper.Map<OrderModel>(order);

            return model;

        }

        private async Task SaveCustomer(PurchaseModel model, SiteUser user)
        {  
            //no return needed at this point
            var customer = await customerService.SaveCustomer(model, user); 
        }


        [HttpPost]
        public async Task<IActionResult> Confirm(PurchaseModel model)
        {
            try
            {
                model.Step = 3;
                return RouteToContinue(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ordering");
            }

            return RedirectToAction("Unable");
        }

        [HttpPost]
        public async Task<IActionResult> Complete(PurchaseModel model)
        {
            try
            {
                //TODO add SAGA make sure cart doesn't clear if order isn't placed! 
                bool success =  await orderService.CompleteOrder(model.Order.Id);
     
                if (success)
                {
                    await shoppingCartService.ClearCart(model.CartId, model.Order.Id);
                    return RedirectToAction("Index", "Home");
                }

                return RouteToContinue(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ordering");
            }

            return RedirectToAction("Unable");
        }
    }
}

