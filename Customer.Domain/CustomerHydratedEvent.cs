using System;
using Shared.Domain;

namespace Customer.Domain
{
    public class CustomerHydratedEvent : IDomainEvent
    {
        public CustomerHydratedEvent(Customer customer)
        {
            this.CustomerId = customer.Id;
            DateTimeEventOccurred = DateTime.Now;
            Customer = customer;
        }

        public Guid CustomerId { get; }
        public DateTime DateTimeEventOccurred { get; }

        public Customer Customer { get; }
    }
}