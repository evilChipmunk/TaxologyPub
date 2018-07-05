using System.Collections.Generic;

namespace ShoppingCart.Domain
{
    public class ProductSaleEmail
    {
        private readonly IEnumerable<ShoppingCart> carts;

        public ProductSaleEmail(IEnumerable<ShoppingCart> carts)
        {
            this.carts = carts;
        }

        public void NotifyCustomers()
        {

        }
    }
}