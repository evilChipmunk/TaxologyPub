using System.Collections.Generic;
using Customer.DTO;

namespace Product.Domain
{
    public class ProductRecommender
    {
        public IEnumerable<Product> GetRecommendedProducts(CustomerDTO customer)
        {
            return new List<Product>();

        }
    }
}