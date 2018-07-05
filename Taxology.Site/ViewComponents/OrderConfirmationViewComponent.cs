using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Service;
using Taxology.Site.Models;

namespace Taxology.Site.ViewComponents
{
    public class OrderConfirmationViewComponent : ViewComponent
    {
        private readonly IRestClient restClient;
        private readonly RoutingConfig routingConfig;
        private readonly IMapper mapper;

        public OrderConfirmationViewComponent(IRestClient restClient, RoutingConfig routingConfig, IMapper mapper)
        {
            this.restClient = restClient;
            this.routingConfig = routingConfig;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(PurchaseModel model)
        {
            return View(model);
        }
    }
}