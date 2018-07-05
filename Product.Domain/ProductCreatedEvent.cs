using System;
using Shared.Domain;

namespace Product.Domain
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public ProductCreatedEvent(Product product)
        {
            this.ProductId = product.Id;
            DateTimeEventOccurred = DateTime.Now;
        }

        public Guid ProductId { get; set; }

        public DateTime DateTimeEventOccurred { get; }
    }
}