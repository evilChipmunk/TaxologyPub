using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging;
using Product.DTO;
using Shared.Service; 


namespace Taxology.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : BaseApiController 
    {
        private readonly IRequestClient<GetAllProductsRequest, GetAllProductsResponse> client;

        public ProductsController(IRequestClientFactory clientFactory, ILoggerFactory loggerFactory) 
            : base(loggerFactory)
        {
            this.client = clientFactory.Create < GetAllProductsRequest, GetAllProductsResponse>();
        }
        protected override ILogger CreateLogger()
        {
            return loggerFactory.CreateLogger<ProductsController>();
        }
          
        [HttpGet(Name = "getall")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> GetAll()
        {  
            return await HandleAsync(async () => await client.Request(new GetAllProductsRequest())); 
        }  
    }
}
