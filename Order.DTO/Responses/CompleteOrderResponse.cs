using Shared.DTO;

namespace Order.DTO
{
    public class CompleteOrderResponse : BaseResponse
    {
        public CompleteOrderResponse()
        {

        }
        public CompleteOrderResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }
    }
}