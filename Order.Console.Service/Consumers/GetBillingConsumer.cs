using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Consumers
{
    public class GetBillingConsumer : IConsumer<GetBillingAddressRequest>
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;

        public GetBillingConsumer(IOrderRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetBillingAddressRequest> context)
        {
            var request = context.Message;

            var order = await repo.GetMostRecentOrderByCustomer(request.CustomerId);

            BillingAddressDTO billingAddressDTO = null;
            if (order != null)
            {
                billingAddressDTO = mapper.Map<BillingAddressDTO>(order.BillingAddress);
            }

            GetBillingAddressResponse response;

            if (billingAddressDTO == null)
            {
                response = new GetBillingAddressResponse(true, ResponseAction.NotFound);
            }
            else
            {
                response = new GetBillingAddressResponse(true, ResponseAction.Found, billingAddressDTO);
            }


            await context.RespondAsync(response);
        }
    }
}
