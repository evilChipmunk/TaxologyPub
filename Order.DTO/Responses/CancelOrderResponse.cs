using Shared.DTO;

namespace Order.DTO
{
    public class CancelOrderResponse : BaseResponse
    {
        public CancelOrderResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }
    }
}