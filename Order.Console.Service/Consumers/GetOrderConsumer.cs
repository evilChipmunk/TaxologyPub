using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Consumers
{
    public class GetOrderConsumer : IConsumer<GetOrderRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public GetOrderConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetOrderRequest> context)
        {
            var request = context.Message;

            var order = await repo.GetOrder(request.OrderId);

            GetOrderResponse response;
            if (order != null)
            {
                var orderDTO = mapper.Map<OrderDTO>(order);

                response = new GetOrderResponse(orderDTO);

            }
            else
            {
                response = new GetOrderResponse(true, ResponseAction.NotFound);
            }

            await context.RespondAsync(response);

        }
    }
}