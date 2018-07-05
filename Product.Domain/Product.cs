using System;
using Shared.Domain;

namespace Product.Domain
{
    public class Product : BaseEntity
    {
        private Product()  
        {

        }
     

        public static Product CreateForDelete(Guid id)
        {
            var product = new Product();
            product.Id = id;
            return product;
        }

        public static Product Create(string name, Money price, string imageLink, ProductCategory category, string description) 
        {
            Guard.ForNullOrEmpty(name, "name");
            Guard.ForNull(price, "price"); 
           
            var product = new Product();
            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Category = category;
            product.ImageLink = imageLink;
            product.State = SaveState.UnSaved;


            DomainEvents.Raise(new ProductCreatedEvent(product));

            return product;
        }


        public static Product CreateExisting(Guid id, string name, Money price, ProductCategory category)
        {
            Guard.ForNullOrEmpty(name, "name");
            Guard.ForNull(price, "price");

            var product = new Product();
            product.Id = id;
            product.Name = name;
            product.Price = price;
            product.Category = category;

            return product;
        }
        public string Name { get; private set; }

        public Money Price { get; private set; }

        public string ImageLink { get; private set; }

        public ProductCategory Category { get; private set; }


        public string Description { get; private set; }
    }
}
