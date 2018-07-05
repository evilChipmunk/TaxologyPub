using System.Threading.Tasks;
using Customer.DTO;
using MassTransit;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;
using ShoppingCart.Service.Consumers;

namespace ShoppingCart.Service.Controllers
{
    public class CustomerCreatedConsumer : IConsumer<ICustomerCreatedNotification>{
        private readonly IShoppingCartRepository repo;
        private readonly IShoppingCartBuilder builder;

        public CustomerCreatedConsumer(IShoppingCartRepository repo, IShoppingCartBuilder builder)
        {
            this.repo = repo;
            this.builder = builder;
        }

        public async Task Consume(ConsumeContext<ICustomerCreatedNotification> context)
        {
            var customer = context.Message.Customer;

            var request = new GetShoppingCartByCustomerRequest(customer.AnonymousId);

            //either get an existing cart or create a new one
            var cartDTO = await builder.GetCartAsync(request);

            var cart = await repo.GetCartByIdAsync(cartDTO.Id);

            cart.UpdateCustomer(customer.AnonymousId, customer.Id);

            await repo.SaveAsync(cart);
             
        } 
    }
}