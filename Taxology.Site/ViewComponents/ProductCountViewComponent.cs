using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using Shared.Authentication;
using Shared.Service;
using ShoppingCart.DTO; 

namespace Taxology.Site.ViewComponents
{
    public class ProductCountViewComponent : ViewComponent
    {

        private readonly IRestClient restClient;
        private readonly IMapper mapper;
        private readonly UserManager<SiteUser> userManager;
        private readonly RoutingConfig routingConfig;
        private readonly IHttpContextAccessor contextAccessor;

        public ProductCountViewComponent(IRestClient restClient, IMapper mapper, UserManager<SiteUser> userManager
            , RoutingConfig routingConfig, IHttpContextAccessor contextAccessor)
        {
            this.restClient = restClient;
            this.mapper = mapper;
            this.userManager = userManager;
            this.routingConfig = routingConfig;
            this.contextAccessor = contextAccessor;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            string routeName = "";
            string routeIdentifier = "";

            var user = await userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrEmpty(user.Name))
            {
                IAnonymousIdFeature feature = HttpContext.Features.Get<IAnonymousIdFeature>();
                if (feature != null)
                {
                    routeIdentifier = feature.AnonymousId; 
                }
            }
            else
            {
                routeIdentifier = user.CustomerId.ToString(); 

            }

            int count = 0;




            try
            {
                // var IpAddress = contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                var response = await this.restClient.Get<GetShoppingCartCountResponse>(
                    routingConfig.ShoppingCartService.URL,
                    routingConfig.ShoppingCartService.ProductCount, routeIdentifier, HeaderAccept.Json);

                count = response.Count;
            }
            catch (Exception e)
            {
            }


            return View(count);
        }
         
    }
}