using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class CreateShoppingCartResponse : BaseResponse
    {
        public CreateShoppingCartResponse( ShoppingCartDTO shoppingCart, bool success, 
            ResponseAction responseAction) : base(success, responseAction)
        {
            ShoppingCart = shoppingCart;
        }

        public ShoppingCartDTO ShoppingCart { get; set; }
    }
}