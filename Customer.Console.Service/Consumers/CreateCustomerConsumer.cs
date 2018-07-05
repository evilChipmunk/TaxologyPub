using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using MassTransit;

namespace Customer.Service
{
    public class CreateCustomerConsumer : IConsumer<ICreateCustomerRequest>
    {
        private readonly ICustomerBuilder builder;
        private readonly ICustomerRepository repo;
        private readonly IMapper mapper;

        public CreateCustomerConsumer(ICustomerBuilder builder, ICustomerRepository repo, IMapper mapper)
        {
            this.builder = builder;
            this.repo = repo;
            this.mapper = mapper;
        }


        public async Task Consume(ConsumeContext<ICreateCustomerRequest> context)
        {
            var customer = builder.Build(context.Message.RegisteredUser);
            customer = await this.repo.SaveAsync(customer);
            var dto = mapper.Map<CustomerDTO>(customer);
             
            var response = new CreateCustomerResponse(dto);


            await context.RespondAsync(response); 
             
        } 
    }
}