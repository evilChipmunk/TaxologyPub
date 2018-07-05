using System;

namespace Order.DTO
{
    public class GetBillingAddressRequest
    {
        public GetBillingAddressRequest(Guid customerId)
        {
            CustomerId = customerId;
        }
        public Guid CustomerId { get; set; }
    }
}