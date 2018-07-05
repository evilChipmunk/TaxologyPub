using System;

namespace Order.DTO
{
    public class GetCustomerOrdersRequest
    {
        public GetCustomerOrdersRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; set; }
    }
}