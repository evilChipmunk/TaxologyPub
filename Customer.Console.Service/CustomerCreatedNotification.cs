using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Customer.Domain;
using Customer.DTO;
using MassTransit;
using MediatR;

namespace Customer.Service
{
    public class CustomerCreatedNotification : INotificationHandler<CustomerCreatedEvent>
    { 
        private readonly IBusControl bus;
        private readonly IMapper mapper;

        public CustomerCreatedNotification(IBusControl bus, IMapper mapper)
        {
            this.bus = bus;
            this.mapper = mapper;
        }
        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var customerDTO = mapper.Map<CustomerDTO>(notification.Customer);
            await bus.Publish<Customer.DTO.ICustomerCreatedNotification>(new {Customer = customerDTO}, cancellationToken);
        }
    }
}