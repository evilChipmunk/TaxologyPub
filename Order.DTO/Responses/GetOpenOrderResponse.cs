using Shared.DTO;

namespace Order.DTO
{
    public class GetOpenOrderResponse : BaseResponse
    {
        public GetOpenOrderResponse()
        {

        }
        public GetOpenOrderResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }

        public GetOpenOrderResponse(OrderDTO orderDTO) : base(true, ResponseAction.Found)
        {
            Order = orderDTO;
        }

        public OrderDTO Order { get; set; }
      
    }
}