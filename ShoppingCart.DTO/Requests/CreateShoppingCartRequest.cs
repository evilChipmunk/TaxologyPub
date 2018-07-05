using System;

namespace ShoppingCart.DTO
{
    public class CreateShoppingCartRequest
    {
        public CreateShoppingCartRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; set; }
    }
}