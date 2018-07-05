using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Authentication;
using Shared.Service;
using ShoppingCart.DTO;
using Taxology.Site.Controllers;

namespace Taxology.Site.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDTO> GetShoppingCart();

        Task<bool> AddProduct(ShoppingCartDTO cart, ShoppingCartProductDTO product);

        Task RemoveProduct(Guid cartId, Guid productId);


        Task<bool> ClearCart(Guid cartId, Guid orderId);
        Task<ShoppingCartDTO> GetShoppingCart(SiteUser user);
    }
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRestClient restClient;
        private readonly IMapper mapper;
        private readonly ShoppingCartRoutingConfig routing;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly AnoymousIdHandler anonHandler;
        private readonly UserManager<SiteUser> userManager;

        public ShoppingCartService(IRestClient restClient, IMapper mapper
            , RoutingConfig routingConfig, UserManager<SiteUser> userManager, IHttpContextAccessor contextAccessor, AnoymousIdHandler anonHandler)
          
        {
            this.restClient = restClient;
            this.mapper = mapper;
            this.routing = routingConfig.ShoppingCartService;
            this.contextAccessor = contextAccessor;
            this.anonHandler = anonHandler;
            this.userManager = userManager;
        }


        public async Task<ShoppingCartDTO> GetShoppingCart()
        {
            string routeIdentifier = "";
            var user = await userManager.GetUserAsync(contextAccessor.HttpContext.User);
 
            if (user == null || user.CustomerId == Guid.Empty)
            {
                routeIdentifier = anonHandler.GetAnonId().ToString();
            }
            else
            {
                routeIdentifier = user.CustomerId.ToString();
            }

            var response = await restClient.Get<GetShoppingCartResponse>(routing.URL,
                routing.GetCart, routeIdentifier, HeaderAccept.Json);

            return response.ShoppingCart; 
        }


        public async Task<ShoppingCartDTO> GetShoppingCart(SiteUser user)
        {
            string routeIdentifier = "";
             
                routeIdentifier = user.CustomerId.ToString();
     

            var response = await restClient.Get<GetShoppingCartResponse>(routing.URL,
                routing.GetCart, routeIdentifier, HeaderAccept.Json);

            return response.ShoppingCart;
        }

        public async Task<bool> AddProduct(ShoppingCartDTO cart, ShoppingCartProductDTO product)
        {   
            var request = new AddProductRequest(cart.Id, product);
             
            var response = await restClient.Put<AddProductRequest, AddProductResponse>(
                routing.URL,
                routing.AddProduct, request, HeaderAccept.Json);

            return response.IsSuccess;
        }


        public async Task RemoveProduct(Guid cartId, Guid productId)
        { 
            var request = new RemoveProductRequest(cartId, productId); 

            var response = await restClient.Delete<RemoveProductRequest, RemoveProductResponse>(routing.URL,
                routing.RemoveProduct, request, HeaderAccept.Json);

            if (!response.IsSuccess)
            {
                //do something
            }
        }


        public async Task<bool> ClearCart(Guid cartId, Guid orderId)
        {
            var request = new ClearCartRequest(cartId, orderId);
            var response = await restClient.Put<ClearCartRequest, ClearCartResponse>(routing.URL,
                routing.ClearCart, request, HeaderAccept.Json);

            return response.IsSuccess;
        }

    }
}

 