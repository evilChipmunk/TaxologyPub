using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Order.Service.Builders;

namespace Order.Service.Consumers
{
    public class CreateOrderConsumer : IConsumer<CreateOrderRequest>
    {
        private readonly IOrderRepository repo;
        private readonly ICustomerRepository custRepo;
        private readonly IOrderBuilder orderBuilder;
        private readonly ICustomerBuilder customerBuilder;
        private readonly IMapper mapper;

        public CreateOrderConsumer(IOrderRepository repo 
            , IOrderBuilder orderBuilder, ICustomerBuilder customerBuilder,
            IMapper mapper)
        {
            this.repo = repo;
            this.custRepo = custRepo;
            this.orderBuilder = orderBuilder;
            this.customerBuilder = customerBuilder;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CreateOrderRequest> context)
        {
            var request = context.Message;
            var customer = await customerBuilder.Build(request);

            var order = orderBuilder.Build(customer, request.BillingAddress, request.Products);
            order = await repo.SaveAsync(order);

            var orderDTO = mapper.Map<OrderDTO>(order);

            var response = new CreateOrderResponse(orderDTO);

            await context.RespondAsync(response);
        }


    }
}