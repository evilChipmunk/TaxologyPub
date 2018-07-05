using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Domain;

namespace Order.Persistency
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(Guid id);
        Task<Customer> SaveAsync(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly OrderContext context;

        public CustomerRepository(OrderContext context)
        {
            this.context = context;
        }


        public async Task<Customer> GetCustomer(Guid id)
        {
            return await context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer> SaveAsync(Customer customer)
        {
            return await context.SaveAndPublishAsync(customer, context.Customers);
        }
    }
}