 
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Shared.Domain;
using Shared.Persistency;

namespace Order.Persistency
{ 
    public class OrderContext : BaseContext
    {
        public OrderContext() : base("orders", null)
        {

        }
        public OrderContext(IMediator mediator) : base("orders", mediator)
        {
        }

        public override Task EnsureSeedData()
        {
            return Task.CompletedTask;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Order>().HasMany(x => x.Products);
            modelBuilder.Entity<Domain.Order>().HasOne(x => x.BillingAddress);

            //            modelBuilder.Entity<Domain.Order>().OwnsOne(x => x.Total);
            //            modelBuilder.Entity<Domain.Order>().OwnsOne(x => x.SubTotal);
            //            modelBuilder.Entity<Domain.Order>().OwnsOne(x => x.Tax);

            //modelBuilder.Entity<Domain.Order>().Ignore(x => x.Total);
            //modelBuilder.Entity<Domain.Order>().Ignore(x => x.SubTotal);
            //modelBuilder.Entity<Domain.Order>().Ignore(x => x.Tax);

            modelBuilder.Entity<Product>().HasKey(d => new { d.Id, d.OrderId }); 
          //  modelBuilder.Entity<Product>().Ignore(x => x.Price);
             
            //  modelBuilder.Entity<Product>().OwnsOne(x => x.Price);
        }

        public DbSet<Order.Domain.Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<BillingAddress> BillingAddress { get; set; }
    }
}
