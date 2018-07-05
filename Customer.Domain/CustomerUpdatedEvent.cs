using System;
using Shared.Domain;

namespace Customer.Domain
{
    public class CustomerUpdatedEvent : IDomainEvent{

        public CustomerUpdatedEvent(Customer customer)
        {
            this.Customer = customer;
            DateTimeEventOccurred = DateTime.Now;
        }

        public Customer Customer { get; }

        public DateTime DateTimeEventOccurred { get; }
    }
}