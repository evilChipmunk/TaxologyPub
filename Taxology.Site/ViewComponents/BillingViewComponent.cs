using System; 
using System.Threading.Tasks;
using AutoMapper; 
using Microsoft.AspNetCore.Mvc;
using Shared.Mapping;
using Taxology.Site.Models;
using Taxology.Site.Services;

namespace Taxology.Site.ViewComponents
{
    public class BillingViewComponent : ViewComponent
    { 
        private readonly IOrderRestService orderService;
        private readonly ICustomerRestService customerService;
        private readonly IMapper mapper;

        public BillingViewComponent(IOrderRestService orderService, ICustomerRestService customerService, IMapper mapper)
        { 
            this.orderService = orderService;
            this.customerService = customerService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(PurchaseModel purchaseModel)
        {
            Guid customerId = purchaseModel.CustomerId;
            BillingInfoModel model;

            var customer = await customerService.GetCustomer(customerId);
            var billing = await orderService.GetDefaultBilling(customerId);

            if (billing != null)
            {
                model = mapper.Map<BillingInfoModel>(billing);

                if (customer != null)
                {
                    model.CustomerId = customer.Id;
                    model.FirstName = customer.FirstName;
                    model.LastName = customer.LastName;
                }
            }
            else
            {
                model = mapper.Map<BillingInfoModel>(customer);
            }
             
            purchaseModel.Billing = model;
            return View(purchaseModel);
        } 
    } 
}