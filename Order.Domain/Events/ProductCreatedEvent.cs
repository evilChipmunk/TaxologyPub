using System;
using Shared.Domain;

namespace Order.Domain
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public ProductCreatedEvent(Product product)
        {
            Product = product;
            DateTimeEventOccurred = DateTime.Now;
        }

        public Product Product { get; }
        public DateTime DateTimeEventOccurred { get; }
    }
}