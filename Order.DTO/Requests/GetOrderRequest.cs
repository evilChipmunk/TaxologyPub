using System;

namespace Order.DTO
{
    public class GetOrderRequest
    {
        public GetOrderRequest()
        {

        }

        public GetOrderRequest(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}