using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Product.DTO;
using Product.Persistency;

namespace Product.Service.Controllers
{
    public class ProductConsumer  : IConsumer<GetAllProductsRequest>
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;

        public ProductConsumer(IProductRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
  
        public async Task Consume(ConsumeContext<GetAllProductsRequest> context)
        { 
            var products = await repo.GetProductsAsync();  
            var productDTOS = mapper.Map<IEnumerable<ProductDTO>>(products);

            GetAllProductsResponse response = new GetAllProductsResponse(productDTOS);

            await context.RespondAsync(response); 
        }
    }
}