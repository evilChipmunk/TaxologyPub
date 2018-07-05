using System.Threading.Tasks;
using Order.Domain;
using Order.DTO;
using Order.Persistency;

namespace Order.Service.Builders
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private readonly ICustomerRepository repo;

        public CustomerBuilder(ICustomerRepository repo)
        {
            this.repo = repo;
        }
        public async Task<Customer> Build(CreateOrderRequest request)
        {
            var customer = await repo.GetCustomer(request.Customer.Id);

            if (customer == null)
            {
                customer = Customer.Create(request.Customer.Id, request.Customer.FirstName, request.Customer.LastName);
                customer = await repo.SaveAsync(customer);
            }

            return customer;
        }
    }
}