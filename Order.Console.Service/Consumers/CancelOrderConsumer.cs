using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Consumers
{
    public class CancelOrderConsumer : IConsumer<CancelOrderRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public CancelOrderConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CancelOrderRequest> context)
        {
            var request = context.Message;

            Domain.Order order = await repo.GetOrder(request.OrderId);
            order.Cancel();
            order = await repo.SaveAsync(order);

            var response = new CancelOrderResponse(true, ResponseAction.Updated);

            await context.RespondAsync(response);
        }
    }
}