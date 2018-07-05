using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Order.DTO;
using Shared.Service;
using Taxology.Site.Models;
using Taxology.Site.Services;

namespace Taxology.Site.ViewComponents
{
    public class OrderPurchaseViewComponent : ViewComponent
    {
        private readonly IOrderRestService restClient;
        private readonly RoutingConfig routingConfig;
        private readonly IMapper mapper;

        public OrderPurchaseViewComponent(IOrderRestService restClient, RoutingConfig routingConfig, IMapper mapper)
        {
            this.restClient = restClient;
            this.routingConfig = routingConfig;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(PurchaseModel model)
        {
            OrderDTO order = await restClient.GetOrder(model.Order.Id);

            model.Order = mapper.Map<OrderModel>(order);
            return View(model);
        }
    }
}