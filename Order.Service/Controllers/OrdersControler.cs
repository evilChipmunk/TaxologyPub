using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.DTO;
using Shared.Service;

namespace Order.Service.Controllers
{
    public class Routing
    {
        public const string Create = "create";
        public const string Get = "get";
        public const string GetCustomerOrders = "customerorders";
        public const string GetOpenOrder = "openorder";
        public const string GetBilling = "billingaddress";
        public const string Complete = "complete";
        public const string Cancel = "cancel";
    }
    [Route("api/orders")]
    public class OrdersControler : BaseApiController
    {
        private readonly IOrderService service;

        public OrdersControler(ILoggerFactory loggerFactory, IOrderService service) : base(loggerFactory)
        {
            this.service = service;
        }
        protected override ILogger CreateLogger()
        {
            return loggerFactory.CreateLogger<OrdersControler>();
        }

        [HttpPost(Name = Routing.Create)]
        [Route(Routing.Create)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        { 
            return await HandleAsync(async () => await service.CreateOrder(request));
        }

        [HttpPut(Name = Routing.Complete)]
        [Route(Routing.Complete)]
        public async Task<IActionResult> Complete([FromBody] CompleteOrderRequest request)
        {
            return await HandleAsync(async () => await service.CompleteOrder(request));
        }


        [HttpPut(Name = Routing.Cancel)]
        [Route(Routing.Cancel)]
        public async Task<IActionResult> Cancel([FromBody] CancelOrderRequest request)
        {
            return await HandleAsync(async () => await service.CancelOrder(request));
        }
         


        [HttpGet(Name = Routing.GetBilling)]
        [Route(Routing.GetBilling)]
        public async Task<IActionResult> GetBillingAddress(Guid customerId)
        { 
            return await HandleAsync(async () => await service.GetMostRecentBillingAddress(new GetBillingAddressRequest(customerId)));
        }


        [HttpGet(Name = Routing.Get)]
        [Route(Routing.Get)]
        public async Task<IActionResult> Get(Guid orderId)
        {
            return Ok();
        }


        [HttpGet(Name = Routing.GetOpenOrder)]
        [Route(Routing.GetOpenOrder)]
        public async Task<IActionResult> GetOpenOrder(Guid customerId)
        {
            return await HandleAsync(async () => await service.GetOpenOrder(new GetOpenOrderRequest(customerId)));
        }


        [HttpGet(Name = Routing.GetCustomerOrders)]
        [Route("customerorders/{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            return await HandleAsync(async () => await service.GetCustomerOrders(new GetCustomerOrdersRequest(customerId)));
        }


        //        [HttpGet]
        //        [Route("list")]
        //        public async Task<IActionResult> GetAllCustomerOrders(Guid customerId)
        //        {
        //            return await HandleAsync(() => await service.)
        //        }
    }
}