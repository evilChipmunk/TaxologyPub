using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class AddProductResponse : BaseResponse
    {
        public AddProductResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }
    }
}