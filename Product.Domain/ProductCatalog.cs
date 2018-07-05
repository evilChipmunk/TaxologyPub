using System.Collections.Generic;
using Customer.DTO;

namespace Product.Domain
{
    public class ProductCatalog
    {
        public IEnumerable<Product> GetProducts()
        {
            return new List<Product>();
        }

        public void AddProduct(Product product)
        {

        }

        public void RateProduct(Product product, Rating rating)
        {
        }

        public List<Product> GetRecommendedProducts(CustomerDTO customer)
        {
            return new List<Product>();
        }
    }
}