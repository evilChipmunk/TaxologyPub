using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTO;
using Shared.Service;
using ShoppingCart.DTO;
using Taxology.WebAPI;

namespace ShoppingCart.Service.Controllers
{ 
    [Produces("application/json")]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : BaseApiController
    {
        private readonly IRequestClientFactory clientFactory;

        public ShoppingCartsController(ILoggerFactory loggerFactory, IRequestClientFactory clientFactory) : base(loggerFactory)
        {
            this.clientFactory = clientFactory; 
        }

        protected override ILogger CreateLogger()
        {
           return base.loggerFactory.CreateLogger<ShoppingCartsController>();
        }



        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(Guid customerId)
        {
            return await HandleAsync(async () =>
            {
                var client = clientFactory.Create<GetShoppingCartByCustomerRequest, GetShoppingCartResponse>();
                return await client.Request(new GetShoppingCartByCustomerRequest(customerId));
            });
        }


        //[HttpPost(Name = "/carts/{ipAddress}")]
        //public async Task<IActionResult> CreateCart(string ipAddress)
        //{
        //    return await HandleAsync(async () =>
        //        await service.CreateCartAsync(new CreateAnonShoppingCartRequest {IpAddress = ipAddress}));
        //}


        [HttpPut]
        [Route("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            return await HandleAsync(async () =>
            {
                var client = clientFactory.Create<AddProductRequest, AddProductResponse>();
                return await client.Request(request); 
            });
        }


        [HttpDelete]
        [Route("removeproduct")]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductRequest request)
        {
            return await HandleAsync(async () =>
            {
                var client = clientFactory.Create<RemoveProductRequest, RemoveProductResponse>();
                return await client.Request(request);
            });
        }

        [HttpPut(Name = "clear")]
      //  [Route("clearcart/{cartId}/{orderId}")]
        [Route("clear")]
        //   public async Task<IActionResult> ClearCart(Guid cartId, Guid orderId)
        public async Task<IActionResult> ClearCart([FromBody] ClearCartRequest request)
        {
            return await HandleAsync(async () =>
            {
                var client = clientFactory.Create<ClearCartRequest, ClearCartResponse>();
                return await client.Request(request);
            });
        }
    }
}