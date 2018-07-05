using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using MediatR;


namespace Customer.Persistency
{

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer.Domain.Customer>> GetCustomersAsync();
        Task<Domain.Customer> GetCustomerByIdAsync(Guid id);
        Task<Domain.Customer> SaveAsync(Domain.Customer customer);
        void Delete(Guid id);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext context;
        private readonly IMediator mediator;

        public CustomerRepository(CustomerContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<Customer.Domain.Customer>> GetCustomersAsync()
        {
            return await context.Customers.ToListAsync();
        }

        public async Task<Domain.Customer> GetCustomerByIdAsync(Guid id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<Domain.Customer> SaveAsync(Domain.Customer customer)
        { 
           return  await context.SaveAndPublishAsync(customer, context.Customers);    
        }

        public void Delete(Guid id)
        {
            var entity = Domain.Customer.CreateForDelete(id);
            context.Delete(entity);
        }
    }
}
