using System;
using Shared.Domain;

namespace Order.Domain
{
    public class Product : BaseEntity
    {
        public static Product Create(Guid id, string name, decimal price, Order order)
        { 
            Guard.ForNull(id, "id");
            Guard.ForNullOrEmpty(name, "name");
            Guard.ForNull(order, "order");

            Product product = new Product();
            product.Id = id;
            product.State = SaveState.UnSaved;

            product.Name = name;
            product.Price = new Money(price).Amount; //see note on order create
            product.Exchange = new Money(price).Exchange;
            product.Quantity = 1;

            product.OrderId = order.Id;

            product.AddEvent(new ProductCreatedEvent(product));

            return product;
        }
          
        public string Exchange { get; private set; }


        public string Name { get; private set; }

        public decimal Price { get; set; }

        public Guid OrderId { get; private set; }

        //allowing this property to be managed outside of the product class
        public int Quantity { get; set; }

    }
}