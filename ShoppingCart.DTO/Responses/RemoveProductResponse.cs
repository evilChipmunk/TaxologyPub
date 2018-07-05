using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class RemoveProductResponse : DeleteResponse
    {
        public RemoveProductResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }

        public RemoveProductResponse() : base(true, ResponseAction.Deleted)
        {

        }
    }
}