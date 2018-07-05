using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using Shared.Authentication;
using Taxology.Site.Models;
using Taxology.Site.Services;

namespace Taxology.Site.Controllers
{
    public class ShoppingCartController : BaseWebController
    { 
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ShoppingCartController(IMapper mapper,
             UserManager<SiteUser> userManager,
            IShoppingCartService shoppingCartService,
            IProductService productService)
        :base(userManager)
        { 
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetProducts();

            var cart = await shoppingCartService.GetShoppingCart();
            var model = mapper.Map<ShoppingCartModel>(cart);

            foreach (var cartProduct in model.Products)
            {
                cartProduct.CartId = cart.Id;

                var foundProduct = products.FirstOrDefault(x => x.Id == cartProduct.Id);
                if (foundProduct != null)
                {
                    cartProduct.ImageLink = foundProduct.ImageLink;
                }
                else
                {
                    cartProduct.ImageLink = "http://placehold.it/120x80";
                } 
            }

            return View(model);
        }

        public async Task<IActionResult> Remove(Guid cartId, Guid productId)
        { 
            await shoppingCartService.RemoveProduct(cartId, productId);

            var cart = await shoppingCartService.GetShoppingCart();
            var model = mapper.Map<ShoppingCartModel>(cart);

            return RedirectToAction("Index"); 
        }


        public async Task<IActionResult> Purchase(Guid cartId)
        {
            return RedirectToAction("Index", "Purchase", new {cartId});
        }


        public IActionResult AddCoupon()
        {
            throw new NotImplementedException();
        }
    }
}