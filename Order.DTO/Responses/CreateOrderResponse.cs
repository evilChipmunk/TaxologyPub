using Shared.DTO;

namespace Order.DTO
{
    public class CreateOrderResponse : CreatedResponse
    {
        public CreateOrderResponse()
        {

        }

        public CreateOrderResponse(OrderDTO order) 
        {
            this.Order = order;
        }
        public CreateOrderResponse(OrderDTO order, string link) : base(link)
        {
            this.Order = order;
        }

        public CreateOrderResponse(bool success, ResponseAction responseAction) 
            : base(success, responseAction)
        {

        }

        public OrderDTO Order { get; set; }
    }
}