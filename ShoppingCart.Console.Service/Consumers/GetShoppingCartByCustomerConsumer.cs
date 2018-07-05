using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Shared.DTO;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Consumers
{

    public class GetShoppingCartByCustomerConsumer : IConsumer<GetShoppingCartByCustomerRequest>
    {
        private readonly IShoppingCartRepository repo;
        private readonly IMapper mapper;
        private readonly IShoppingCartBuilder builder;

        public GetShoppingCartByCustomerConsumer(IShoppingCartRepository repo, IMapper mapper
            , IShoppingCartBuilder builder)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.builder = builder;
        }
        public async Task Consume(ConsumeContext<GetShoppingCartByCustomerRequest> context)
        {
            var request = context.Message;
               
            var cart = await builder.GetCartAsync(request);

            var response = new GetShoppingCartResponse(cart, true, ResponseAction.Found);
              
            await context.RespondAsync(response);
        }


    }
}