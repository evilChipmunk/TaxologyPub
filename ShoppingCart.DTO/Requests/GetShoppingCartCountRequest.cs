using System;

namespace ShoppingCart.DTO
{
    public class GetShoppingCartCountRequest
    {
        public GetShoppingCartCountRequest(Guid customerId)
        {
            this.CustomerId = customerId;
        }

        public Guid CustomerId { get; set; }
    }
}