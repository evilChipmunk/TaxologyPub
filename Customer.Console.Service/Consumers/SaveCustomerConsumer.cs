using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using MassTransit;
using Shared.DTO;

namespace Customer.Service
{
    public class SaveCustomerConsumer : IConsumer<ISaveCustomerRequest>
    {

        private readonly ICustomerBuilder builder;
        private readonly ICustomerRepository repo;
        private readonly IMapper mapper;

        public SaveCustomerConsumer(ICustomerBuilder builder, ICustomerRepository repo, IMapper mapper)
        {
            this.builder = builder;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ISaveCustomerRequest> context)
        {

            var request = context.Message;
            var customer = builder.Build(request.Customer);

            customer.UpdateAddress(request.Customer.Address1, request.Customer.Address2, request.Customer.ZipCode,
                request.Customer.Country, request.Customer.StateAbbreviation);

            customer = await this.repo.SaveAsync(customer);
            var dto = mapper.Map<CustomerDTO>(customer);


            var response = mapper.Map<SaveCustomerResponse>(dto);
            response.ResponseAction = ResponseAction.Updated;
         
            await context.RespondAsync(response);

        }

    }
}