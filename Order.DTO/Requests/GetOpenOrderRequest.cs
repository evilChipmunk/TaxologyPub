using System;

namespace Order.DTO
{   
    public class GetOpenOrderRequest
    {
        public GetOpenOrderRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; set; } 
    }
}
