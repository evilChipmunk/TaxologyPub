using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.DTO;
using Product.Persistency;

namespace Product.Service.Controllers
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<GetAllProductsResponse> GetProducts(GetAllProductsRequest request)
        {
            var products = await repo.GetProductsAsync();  
            var productDTOS = mapper.Map<IEnumerable<ProductDTO>>(products);

            GetAllProductsResponse response = new GetAllProductsResponse(productDTOS);
            return response;
        }
    }
}