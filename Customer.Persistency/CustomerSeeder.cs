using System.Linq;
using System.Threading.Tasks;
using Shared.Domain;

namespace Customer.Persistency
{
    public class CustomerSeeder
    {
        private CustomerContext context;

        public CustomerSeeder(CustomerContext context)
        {
            this.context = context;
        }
        public async Task SeedData()
        {

            //var customerCount = context.Customers.Count();

            //if (customerCount != 0)
            //{
            //    return;
            //}

            //Domain.Customer cust = Domain.Customer.Create("Anthony", "West", "a@w.com");
            //Domain.Customer cust2 = Domain.Customer.Create("Michael", "West", "m@w.com", "972", States.GetAbbreviation("texas"));

            //await context.AddAsync(cust);
            //await context.AddAsync(cust2);

            await context.SaveChangesAsync();
        }
    }
}