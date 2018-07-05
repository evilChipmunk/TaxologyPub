using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Consumers
{
    public class GetOpenOrderConsumer : IConsumer<GetOpenOrderRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public GetOpenOrderConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetOpenOrderRequest> context)
        {
            var request = context.Message;

            var order = await repo.GetOpenOrderByCustomer(request.CustomerId);

            GetOpenOrderResponse response;
            if (order != null)
            {
                var orderDTO = mapper.Map<OrderDTO>(order);

                response = new GetOpenOrderResponse(orderDTO);

            }
            else
            {
                response = new GetOpenOrderResponse(true, ResponseAction.NotFound);
            }

            await context.RespondAsync(response);

        }
    }
}