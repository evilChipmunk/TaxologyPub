using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTO;
using Shared.Service;
using ShoppingCart.DTO;

namespace ShoppingCart.Service.Controllers
{ 
    [Produces("application/json")]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : BaseApiController
    {
        private readonly IShoppingCartService service;

        public ShoppingCartsController(ILoggerFactory loggerFactory, IShoppingCartService service) : base(loggerFactory)
        {
            this.service = service;
        }

        protected override ILogger CreateLogger()
        {
           return base.loggerFactory.CreateLogger<ShoppingCartsController>();
        }



        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(Guid customerId)
        {
            return await HandleAsync(async () => await service.GetCartAsync(new GetShoppingCartByCustomerRequest(customerId)));
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
            return await HandleAsync(async () => await service.AddProduct(request));
        }


        [HttpDelete]
        [Route("removeproduct")]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductRequest request)
        {
            return await HandleAsync(async () => await service.RemoveProduct(request));
        }

        [HttpPut(Name = "clear")]
      //  [Route("clearcart/{cartId}/{orderId}")]
        [Route("clear")]
        //   public async Task<IActionResult> ClearCart(Guid cartId, Guid orderId)
        public async Task<IActionResult> ClearCart([FromBody] ClearCartRequest request)
        {
            return await HandleAsync(async () => await service.ClearCart(request));
        }
    }
}