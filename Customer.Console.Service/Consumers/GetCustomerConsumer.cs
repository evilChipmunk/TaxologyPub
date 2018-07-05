using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using MassTransit;
using Shared.DTO;

namespace Customer.Service
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
}