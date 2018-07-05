using System;
using Shared.Domain;

namespace Order.Domain
{
    public class OrderConfirmedEvent : IDomainEvent
    {
        public OrderConfirmedEvent(Order order)
        {
            Order = order;
            DateTimeEventOccurred = DateTime.Now;
        }

        public Order Order { get; }
        public DateTime DateTimeEventOccurred { get; }
    }
}