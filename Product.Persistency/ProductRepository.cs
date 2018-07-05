using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace Product.Persistency
{
    public interface IProductRepository
    {
        Task<IEnumerable<Domain.Product>> GetProductsAsync();
        Task<Domain.Product> GetProductByIdAsync(Guid id);
        Task<Domain.Product> SaveAsync(Domain.Product entity);
        void Delete(Guid id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext context;

        public ProductRepository(ProductContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Domain.Product> GetProductByIdAsync(Guid id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<Domain.Product> SaveAsync(Domain.Product entity)
        {
            return await context.SaveAndPublishAsync(entity, context.Products);

        }

        public void Delete(Guid id)
        {
            var entity = Domain.Product.CreateForDelete(id);
            context.Delete(entity); 
        }
    }
}
