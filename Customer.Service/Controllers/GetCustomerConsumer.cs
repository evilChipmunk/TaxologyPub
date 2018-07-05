using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using MassTransit;
using Shared.DTO;

namespace Customer.Service.Controllers
{
    public class GetCustomerConsumer : IConsumer<IGetCustomerRequest>
    {
        private readonly ICustomerRepository repo;
        private readonly IMapper mapper; 
 
        public GetCustomerConsumer(ICustomerRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper; 
        }
        public async Task Consume(ConsumeContext<IGetCustomerRequest> context)
        {
            GetCustomerResponse response;

            var customer = await repo.GetCustomerByIdAsync(context.Message.Id);
            if (customer == null)
            {
                response = new GetCustomerResponse(false, ResponseAction.NotFound);
            }
            else
            {
                var dto = mapper.Map<CustomerDTO>(customer);
                response = new GetCustomerResponse(dto);

            }
             
            await context.RespondAsync<GetCustomerResponse>(response);
        }
    }

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