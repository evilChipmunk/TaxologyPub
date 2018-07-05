using System.Threading.Tasks;
using MassTransit;
using Shared.DTO;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Consumers
{
    public class RemoveProductConsumer : IConsumer<RemoveProductRequest>
    {
        private readonly IShoppingCartRepository repo;

        public RemoveProductConsumer(IShoppingCartRepository repo)
        {
            this.repo = repo;
        }
        public async Task Consume(ConsumeContext<RemoveProductRequest> context)
        {
            var request = context.Message;
             
            var cart = await repo.GetCartByIdAsync(request.CartId);
            cart.RemoveProduct(request.ProductId);

            await repo.SaveAsync(cart);

            var response = new RemoveProductResponse(true, ResponseAction.Deleted);

            await context.RespondAsync(response);
        }
    }
}