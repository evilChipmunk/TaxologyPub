using System;
using Shared.Domain;

namespace Order.Domain
{
    public class BillingAddressCreatedEvent : IDomainEvent
    {
        public BillingAddressCreatedEvent(BillingAddress billingAddress)
        {
            BillingAddress = billingAddress;
            DateTimeEventOccurred = DateTime.Now;
        }

        public BillingAddress BillingAddress { get; }
        public DateTime DateTimeEventOccurred { get; }
    }
}