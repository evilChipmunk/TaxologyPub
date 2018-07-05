using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Domain;
using Shared.Domain;
using Shared.Persistency;

namespace Product.Persistency
{
    public class ProductContext : BaseContext 
    {
        public ProductContext() : base("products", null)
        {

        }

        public ProductContext(IMediator mediator) : base( "products", mediator)
        {
        }

        public override async Task EnsureSeedData()
        { 
          
            int count = Products.Count();
            if (  count > 0)
            {
                return;
            }
            var filing = Domain.Product.Create("Tax Filing", 
                new Money(25), 
                "images/help.jpg", 
                ProductCategory.Filing,
                "You should file your taxes with us and save!");

            Products.Add(filing);


            var advice = Domain.Product.Create("Tax Advice",
                new Money(50),
                "images/consultation.jpg",
                ProductCategory.TaxConsulting,
                "Don't want to figure it all out? We can do it for you.");

            Products.Add(advice);


            var defense = Domain.Product.Create("Audit Defense",
                new Money(2500),
                "images/justice.jpg",
                ProductCategory.AuditDefense,
                "Tried to do it on your own and messed it all up huh? No problem. We've got your back.");

            Products.Add(defense);

            var appraisal = Domain.Product.Create("Appraisal",
                new Money(2500),
                "images/coins.jpg",
                ProductCategory.Appraisal,
                "So now you're going to have to sell something to pay for the arrears (and the lawyers)? Easy enough, let us tell you how much its worth.");

            Products.Add(appraisal);


            SaveChanges(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product.Domain.Product>().OwnsOne(c => c.Price);
            modelBuilder.Entity<Domain.Product>().Ignore(b => b.State);
        }

        public DbSet<Domain.Product> Products { get; set; }
    }
}
