using System;

namespace ShoppingCart.DTO
{
    public class RemoveProductRequest
    {
        public RemoveProductRequest(Guid cartId, Guid productId)
        {
            CartId = cartId;
            ProductId = productId;
        }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }
}