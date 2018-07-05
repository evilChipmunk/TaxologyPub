using System;
using Shared.Domain;

namespace ShoppingCart.Domain
{ 
    public class ShoppingCartProduct : BaseEntity
    {
        public static ShoppingCartProduct Create(Guid productId, string name, ShoppingCart cart, Money price)
        {
            var shoppingCartProduct = new ShoppingCartProduct();
            shoppingCartProduct.Id = Guid.NewGuid();
            shoppingCartProduct.State = SaveState.UnSaved;

            shoppingCartProduct.ProductId = productId;
            shoppingCartProduct.Name = name;
            shoppingCartProduct.ShoppingCartId = cart.Id;
            shoppingCartProduct.Price = price;

            return shoppingCartProduct;
        }
        public Guid ProductId { get; private set; }
        public Guid ShoppingCartId { get; private set; }
        public string Name { get; private set; }
        public Money Price { get; private set; }

        public void UpdateName(string newName)
        {
            Name = newName;
        }

        public void UpdatePrice(Money newPrice)
        {
            Price = newPrice;
        }
    }
}
