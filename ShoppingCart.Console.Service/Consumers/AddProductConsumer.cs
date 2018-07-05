 
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Shared.Domain;
using Shared.DTO;
using ShoppingCart.Domain;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Consumers
{
    public class AddProductConsumer : IConsumer<AddProductRequest>
    {
        private readonly IShoppingCartRepository repo; 

        public AddProductConsumer(IShoppingCartRepository repo)
        {
            this.repo = repo; 
        }
        public async Task Consume(ConsumeContext<AddProductRequest> context)
        {
            var request = context.Message;

            var cart = await repo.GetCartByIdAsync(request.CartId);
            var product = request.Product;

            //double dispatch pattern
            cart.AddProduct(ShoppingCartProduct.Create(product.Id, product.Name, cart, new Money(product.Price.Amount)));

            await repo.SaveAsync(cart);

            var response =  new AddProductResponse(true, ResponseAction.Updated);

            await context.RespondAsync(response);
        }
    }
}
