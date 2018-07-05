using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Persistency;

namespace ShoppingCart.Persistency
{
    public class ShoppingCartContext : BaseContext
    {
        public ShoppingCartContext() : base("shoppingcarts", null)
        {

        }
        public ShoppingCartContext(IMediator mediator) : base("shoppingcarts", mediator)
        {
        }

        public override Task EnsureSeedData()
        {
            return Task.CompletedTask;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.ShoppingCartProduct>().OwnsOne(c => c.Price);
          //  modelBuilder.Entity<Domain.ShoppingCart>().OwnsOne(c => c.Total);
            modelBuilder.Entity<Domain.ShoppingCart>().Ignore(x => x.Total);
             
           // DbContextOptionsBuilder.EnableSensitiveDataLogging();


           //modelBuilder.Entity<Domain.ShoppingCart>().Ignore(x => x.State);
           //modelBuilder.Entity<Domain.ShoppingCartProduct>().Ignore(x => x.State);


           

        }

        public DbSet<ShoppingCart.Domain.ShoppingCart> ShoppingCarts { get; set; } 

        
    }
}
