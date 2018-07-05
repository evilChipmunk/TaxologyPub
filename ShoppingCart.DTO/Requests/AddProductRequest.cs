using System;

namespace ShoppingCart.DTO
{
    public class AddProductRequest
    {
        public AddProductRequest(Guid cartId, ShoppingCartProductDTO product)
        {
            this.CartId = cartId;
            this.Product = product;
        }
        public Guid CartId { get; set; }
        //  public Guid ProductId { get; set; }
        public ShoppingCartProductDTO Product { get; set; }
    }
}