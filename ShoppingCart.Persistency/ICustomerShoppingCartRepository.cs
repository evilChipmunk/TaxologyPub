using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Domain;

namespace ShoppingCart.Persistency
{
    public interface IShoppingCartRepository
    {
        Task<Domain.ShoppingCart> GetCartByCustomerAsync(Guid customerId);
        Task<Domain.ShoppingCart> GetCartByIdAsync(Guid cartId);
        Task<IEnumerable<Domain.ShoppingCart>> GetCartsByProductAsync(Guid productId);


        Task<int> GetCartProductCountAsync(Guid customerId);

        Task SaveAsync(Domain.ShoppingCart cart);
    }
}