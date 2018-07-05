using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Consumers
{
    public class CompleteOrderConsumer : IConsumer<CompleteOrderRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public CompleteOrderConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CompleteOrderRequest> context)
        {
            var request = context.Message;

            Domain.Order order = await repo.GetOrder(request.OrderId);
            order.Confirm();
            order = await repo.SaveAsync(order);

            var response = new CompleteOrderResponse(true, ResponseAction.Updated);

            await context.RespondAsync(response);

        }
    }
}