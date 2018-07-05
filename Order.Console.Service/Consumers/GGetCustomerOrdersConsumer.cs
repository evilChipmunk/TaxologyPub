using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;

namespace Order.Service.Consumers
{
    public class GetCustomerOrdersConsumer : IConsumer<GetCustomerOrdersRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public GetCustomerOrdersConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCustomerOrdersRequest> context)
        {
            var request = context.Message;

            var orders = await repo.GetCustomerOrders(request.CustomerId);

            GetCustomerOrdersResponse response;
            if (orders != null)
            {
                var orderDTOs = mapper.Map<IEnumerable<OrderDTO>>(orders);

                response = new GetCustomerOrdersResponse(orderDTOs);

            }
            else
            {
                response = new GetCustomerOrdersResponse();
            }

            await context.RespondAsync(response);
        }
    }
}