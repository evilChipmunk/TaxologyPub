  
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Product.DTO;
using Shared.Service;

namespace Taxology.Site.Services
{ 
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<IEnumerable<ProductDTO>> SearchForProducts(string name);
    }
    public class ProductService : IProductService
    {
        private readonly IRestClient client; 
        private readonly ProductServiceRoutingConfig routing;

        public ProductService(IRestClient client, RoutingConfig routingConfig)
        {
            this.client = client; 
            this.routing = routingConfig.ProductService;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        { 
            IEnumerable<ProductDTO> products = new List<ProductDTO>();

            await client.Get<GetAllProductsResponse>(routing.URL, routing.GetAllProducts,
                success: (response) => { products = response.Products; },
            error: (error) =>
            {
                //do something
            },
            headerAccept:  HeaderAccept.None);


            return products;

        }
         
        public async Task<IEnumerable<ProductDTO>> SearchForProducts(string name)
        {
            if (name == null) return new List<ProductDTO>();
            var products = await GetProducts();

            return products.Where(x =>
                x.Name.ToLower().Equals(name.ToLower())
                || x.Name.ToLower().Contains(name.ToLower())
                || x.Description.ToLower().Contains(name.ToLower())
            );
        } 
    }
}
