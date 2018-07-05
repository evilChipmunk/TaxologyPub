using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging;
using Product.DTO;
using Shared.Service;


namespace Product.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : BaseApiController 
    {
        private readonly IProductService service;

        public ProductsController(IProductService service, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.service = service;
        }
        [HttpGet(Name = "getall")]
        public async Task<IActionResult> GetAll()
        { 

            return await HandleAsync(async () =>
            {
                var response = await service.GetProducts(new GetAllProductsRequest()); 
                return response;

            });

        }





//        // GET api/values
//        [HttpGet(Name = "getall")]
//        public IEnumerable<ProductDTO> Get()
//        {
//            return new List<ProductDTO>
//            {
//                new ProductDTO {Id = Guid.NewGuid(), Name = "Tax Filing"}
//            };
//        }

        //// GET api/values/5
        //[HttpGet("{id}", Name = "get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        protected override ILogger CreateLogger()
        {
            return loggerFactory.CreateLogger<ProductsController>();
        }
    }
}
