using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class GetShoppingCartResponse : BaseResponse
    {
        public GetShoppingCartResponse()
        {

        }
        public ShoppingCartDTO ShoppingCart { get; set; }

        public GetShoppingCartResponse(ShoppingCartDTO cart, bool success, ResponseAction responseAction) : base(success, responseAction)
        {
            ShoppingCart = cart;
        }

        public GetShoppingCartResponse(bool success, ResponseAction action) : base(success, action)
        { 
        }
    }
}