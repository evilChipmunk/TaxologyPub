using Shared.DTO;

namespace Order.DTO
{
    public class GetOrderResponse : BaseResponse
    {
        public GetOrderResponse()
        {

        }
        public GetOrderResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }

        public GetOrderResponse(OrderDTO orderDTO) : base(true, ResponseAction.Found)
        {
            Order = orderDTO;
        }

        public OrderDTO Order { get; set; }

    }
}