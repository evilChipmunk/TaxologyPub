using System;
using Shared.Domain;

namespace Customer.Domain
{
    public class CustomerCreatedEvent : IDomainEvent
    {
        public CustomerCreatedEvent(Customer customer)
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