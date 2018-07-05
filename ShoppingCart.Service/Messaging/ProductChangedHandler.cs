using System;
using System.Linq;
using Product.DTO;
using Shared.Domain;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Controllers
{
    public class ProductChangedHandler
    {
//        private readonly IShoppingCartProductRepository productRepo;
        private readonly IShoppingCartRepository cartRepo;

        public ProductChangedHandler(IShoppingCartRepository cartRepo)
        { 
            this.cartRepo = cartRepo;
        }
        public async void Handle(ProductDTO product)
        {
            var carts = await cartRepo.GetCartsByProductAsync(product.Id);
            foreach (var cart in carts)
            {
                // var shoppingCartProduct = await productRepo.GetProductAsync(product.Id);
                var shoppingCartProduct = cart.Products.FirstOrDefault(x => x.Id == product.Id);

                if (shoppingCartProduct == null) break;

                if (shoppingCartProduct.Name != product.Name)
                {
                    shoppingCartProduct.UpdateName(product.Name);
                }

                string message = "";
                if (!shoppingCartProduct.Price.Amount.Equals(product.Price.Amount))
                {

                    string priceChange = "";
                    if (shoppingCartProduct.Price.Amount < product.Price.Amount)
                    {
                        priceChange = "Act fast prices are going up! A product you want has changed price.";
                        //send customer an email that price has RAISED
                    }
                    else
                    {
                        priceChange = "And a recent product you were interested in has dropped in price!";
                        //send customer an email that price has DROPPED
                    }

                    message =
                        $"Dear Customer {cart.CustomerId}, you're not just a number, you're a guid. {priceChange}";


                    shoppingCartProduct.UpdatePrice(new Money(product.Price.Amount));

                }

                await cartRepo.SaveAsync(cart);
                //this could potentially spam customers if more than one product in their cart changed
                RaiseEventToEmail(cart.CustomerId, message);
            }

            //  await productRepo.SaveAsync(shoppingCartProduct);

        }

        private void RaiseEventToEmail(Guid customerId, string message)
        {
            //put message in a queue that is picked up by Customer Service (the service handling emails to customers)
        }
    }
}