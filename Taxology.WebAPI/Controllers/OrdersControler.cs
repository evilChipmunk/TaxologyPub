using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.DTO;
using Shared.Service;
using Taxology.WebAPI;

namespace Order.Service.Controllers
{
    public class Routing
    {
        public const string CreateOrder = "createorder";
        public const string GetOrder = "getorders";
        public const string GetCustomerOrders = "customerorders";
        public const string GetOpenOrder = "openorder";
        public const string GetBilling = "billingaddress";
        public const string Complete = "complete";
        public const string Cancel = "cancel";
    }
    [Route("api/orders")]
    public class OrdersControler : BaseApiController
    {
        private readonly IRequestClientFactory getClientFactory; 

        public OrdersControler(ILoggerFactory loggerFactory
            , IRequestClientFactory getClientFactory) : base(loggerFactory)
        {
            this.getClientFactory = getClientFactory; 
        }
        protected override ILogger CreateLogger()
        {
            return loggerFactory.CreateLogger<OrdersControler>();
        }

        [HttpPost(Name = Routing.CreateOrder)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {

            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<CreateOrderRequest, CreateOrderResponse>();

                var response = await client.Request(request);
                response.RequestUri = Url.Link(Routing.GetOrder, new { id = response.Order.Id});
                return response;
            });
        }

        [HttpPut(Name = Routing.Complete)]
        [Route("complete")]
        public async Task<IActionResult> Complete([FromBody] CompleteOrderRequest request)
        {
            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<CompleteOrderRequest, CompleteOrderResponse>();
                return await client.Request(request); 
            });
        }


        //[HttpPut(Name = Routing.Cancel)]
        //[Route("cancel")]
        //public async Task<IActionResult> Cancel([FromBody] CancelOrderRequest request)
        //{
        //    return await HandleAsync(async () =>
        //    {
        //        var client = getClientFactory.Create<CancelOrderRequest, CancelOrderResponse>();
        //        return await client.Request(request); 
        //    });
        //}
         


        [HttpGet(Name = Routing.GetBilling)]
        [Route("billingaddress/{customerId}")]
        public async Task<IActionResult> GetBillingAddress(Guid customerId)
        { 
            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<GetBillingAddressRequest, GetBillingAddressResponse>();
                return await client.Request(new GetBillingAddressRequest(customerId)); 
            });
        }


        [HttpGet(Name = Routing.GetOrder)]
        [Route("{orderId}")]
        public async Task<IActionResult> Get(Guid orderId)
        {
            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<GetOrderRequest, GetOrderResponse>();
                return await client.Request(new GetOrderRequest(orderId));
            });
        }


        [HttpGet(Name = Routing.GetOpenOrder)]
        [Route("open/{customerId}")]
        public async Task<IActionResult> GetOpenOrder(Guid customerId)
        {
            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<GetOpenOrderRequest, GetOpenOrderResponse>();
                return await client.Request(new GetOpenOrderRequest(customerId)); 
            });
        }


        [HttpGet(Name = Routing.GetCustomerOrders)]
        [Route("customerorders/{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            return await HandleAsync(async () =>
            {
                var client = getClientFactory.Create<GetCustomerOrdersRequest, GetCustomerOrdersResponse>();
                return await client.Request(new GetCustomerOrdersRequest(customerId)); 
            });
        } 
    }
}