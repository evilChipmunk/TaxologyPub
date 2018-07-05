using System;

namespace ShoppingCart.DTO
{
    public class ClearCartRequest
    {
        public ClearCartRequest(Guid cartId, Guid orderId)
        {
            this.CartId = cartId;
            this.OrderId = orderId;
        }

        public Guid OrderId { get; set; }

        public Guid CartId { get; set; }
    }
}