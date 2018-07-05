using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace ShoppingCart.Domain
{
    public class ShoppingCart : BaseEntity
    {
        protected ShoppingCart()  
        {
            this.products = new List<ShoppingCartProduct>(); 
        }


        private List<ShoppingCartProduct> products; 
      
        public Guid CustomerId { get; private set; } 
        public Guid OrderId { get; private set; }

        public Money Total
        {
            get { return CalculateTotal(); }
        }
        public IEnumerable<ShoppingCartProduct> Products
        {
            get { return products.AsEnumerable();  } set { products = new List<ShoppingCartProduct>(value); }
        }

        private Money CalculateTotal()
        {
            decimal price = 0;

            if (products != null)
            { 
                foreach (var product in products)
                {
                    price += product.Price.Amount;
                }
            }

            return new Money(price);
        }

        public void AddProduct(ShoppingCartProduct product)
        {
            //only add the product if it doesn't already exists in the list
            if (products.All(x => x.Name != product.Name))
            {
                products.Add(product); 
            } 
        }

        public void RemoveProduct(Guid productId)
        {
            this.products.Remove(this.products.FirstOrDefault(x => x.ProductId == productId)); 
        }

        public void EmptyCart()
        {
            this.products.Clear(); 
        }

        public void UpdateCustomer(Guid anonymousId, Guid customerId)
        {
            if (this.CustomerId == anonymousId)
            {
                this.CustomerId = customerId;
            }
            else
            {
                throw new Exception("The customer is being set on the wrong cart");
            } 
        }

        public void UpdateOrder(Guid orderId)
        {
            Guard.ForNullOrEmpty(orderId, "orderId");

            if (this.OrderId != Guid.Empty)
            {
                throw new Exception("The order has already been associated to a purchase");
            }
            this.OrderId = orderId;
        }


        public static ShoppingCart Create(Guid customerId)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.Id = Guid.NewGuid();
            cart.CustomerId = customerId;
            cart.OrderId = Guid.Empty;
            cart.State = SaveState.UnSaved; 

            return cart;
        }
        public static ShoppingCart Create(Guid customerId, IEnumerable<ShoppingCartProduct> products)
        {
            ShoppingCart cart = Create(customerId); 
            foreach (var product in products)
            {
                cart.AddProduct(product);
            }

            return cart;
        }
        
    }
}