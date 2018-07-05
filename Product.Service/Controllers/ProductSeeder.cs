using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Domain;
using Product.Persistency;
using Shared.Domain;

namespace Product.Service.Controllers
{
    public class ProductSeeder
    {
        private readonly ProductContext context;

        public ProductSeeder(ProductContext context)
        {
            this.context = context;
        }
        public async Task SeedData()
        {
            try
            {
               string con = context.Database.GetDbConnection().ConnectionString;
                int i = context.Products.Count();
            }
            catch (Exception ex)
            {

            }
            var productCount = context.Products.Count();

            if (productCount != 0)
            {
                return;
            }

            var filingProduct = Domain.Product.Create("Tax Filing", new Money(25), "http://placehold.it/350x260", ProductCategory.Filing, "You should file your taxes with us and save!");
            var adviceProduct = Domain.Product.Create("Accounting Advice", new Money(50), "http://placehold.it/350x260", ProductCategory.TaxConsulting, "Don't want to figure it all out? We can do it for you.");
            var defenseProduct = Domain.Product.Create("Audit Defense", new Money(2500), "http://placehold.it/350x260", ProductCategory.AuditDefense, "Tried to do it on your own and messed it all up huh? No problem. We've got your back.");
            var appraisalProduct = Domain.Product.Create("Appraisal", new Money(500), "http://placehold.it/350x260", ProductCategory.Appraisal, "So now you're going to have to sell something to pay for the arrears? Easy enough, let us tell you how much its worth.");

            await context.Products.AddAsync(filingProduct);
            await context.Products.AddAsync(adviceProduct);
            await context.Products.AddAsync(defenseProduct);
            await context.Products.AddAsync(appraisalProduct);
             
                await context.SaveChangesAsync();
          
        }
    }
}