using System.Threading.Tasks;
using MassTransit;
using Shared.DTO;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Consumers
{
    public class ClearCartConsumer : IConsumer<ClearCartRequest>
    {
        private readonly IShoppingCartRepository repo;

        public ClearCartConsumer(IShoppingCartRepository repo)
        {
            this.repo = repo;
        }
        public async Task Consume(ConsumeContext<ClearCartRequest> context)
        {
            var request = context.Message;
             
            var cart = await repo.GetCartByIdAsync(request.CartId);
            cart.UpdateOrder(request.OrderId);
            await repo.SaveAsync(cart);

            var response = new ClearCartResponse(true, ResponseAction.Updated);

            await context.RespondAsync(response);
        }
    }
}