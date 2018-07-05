using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Persistency;

namespace Customer.Persistency
{
    public class CustomerContext : BaseContext
    {
        public CustomerContext() : base("customers", null)
        {

        }
        public CustomerContext(IMediator mediator) : base("customers", mediator)
        { 
        }

        public override Task EnsureSeedData()
        {
            return Task.CompletedTask;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
//            modelBuilder.Entity<Domain.Customer>()
//            .Ignore(b => b.State);
            modelBuilder.Entity<Domain.Customer>().Ignore(x => x.AnonymousId); // no reason to save this explicitly
        } 
        public DbSet<Customer.Domain.Customer> Customers { get; set; }
         
    }
}
