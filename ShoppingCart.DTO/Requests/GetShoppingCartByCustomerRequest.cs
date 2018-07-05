using System;

namespace ShoppingCart.DTO
{
    public class GetShoppingCartByCustomerRequest
    {
        public GetShoppingCartByCustomerRequest(Guid customerId)
        {
            this.CustomerId = customerId;
        }

        public Guid CustomerId { get; set; }
    }
}