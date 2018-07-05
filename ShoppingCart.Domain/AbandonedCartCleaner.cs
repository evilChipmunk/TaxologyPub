using System.Collections.Generic;

namespace ShoppingCart.Domain
{
    public class AbandonedCartCleaner
    {
        private readonly IEnumerable<ShoppingCart> carts;

        public AbandonedCartCleaner(IEnumerable<ShoppingCart> carts)
        {
            this.carts = carts;
        }

        public void Clean()
        {

        }
    }
}