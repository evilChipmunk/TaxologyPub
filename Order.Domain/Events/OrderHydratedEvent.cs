using System;
using Shared.Domain;

namespace Order.Domain
{
    public class OrderHydratedEvent : IDomainEvent
    {
        public OrderHydratedEvent(Order order)
        {
            Order = order;
            DateTimeEventOccurred = DateTime.Now;
        }

        public Order Order { get; }
        public DateTime DateTimeEventOccurred { get; }
    }
}