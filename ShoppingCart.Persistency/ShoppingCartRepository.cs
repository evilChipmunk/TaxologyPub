using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace ShoppingCart.Persistency
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShoppingCartContext context;

        public ShoppingCartRepository(ShoppingCartContext context)
        {
            this.context = context;
        }
        public async Task<Domain.ShoppingCart> GetCartByCustomerAsync(Guid customerId)
        { 
            return await context.ShoppingCarts
                .Include(x => x.Products)
                .ThenInclude(x => x.Price)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.OrderId == Guid.Empty);
        }

        public async Task<Domain.ShoppingCart> GetCartByIdAsync(Guid cartId)
        {
            return await context.ShoppingCarts
                .Include(x => x.Products)
                .ThenInclude(x => x.Price)
                .FirstOrDefaultAsync(x => x.Id == cartId);
        }

        public async Task<IEnumerable<Domain.ShoppingCart>> GetCartsByProductAsync(Guid productId)
        {
            return await context.ShoppingCarts
                .Include(x => x.Products)
                .ThenInclude(x => x.Price)
                .Where(x => x.Products.Any(y => y.Id == productId))
                .ToListAsync();
        }

        public async Task<int> GetCartProductCountAsync(Guid customerId)
        {
            var count = await context.ShoppingCarts
                .Where(o => o.CustomerId == customerId)
                .SelectMany(o => o.Products)
                .CountAsync();

            return count;
        }

        public async Task SaveAsync(Domain.ShoppingCart cart)
        {
            await context.SaveAndPublishAsync(cart, context.ShoppingCarts);
        }
    }
}