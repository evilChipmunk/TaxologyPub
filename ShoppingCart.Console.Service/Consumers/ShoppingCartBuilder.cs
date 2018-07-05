using System.Threading.Tasks;
using AutoMapper;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service.Consumers
{

    public interface IShoppingCartBuilder
    {
        Task<ShoppingCartDTO> GetCartAsync(GetShoppingCartByCustomerRequest request);
    }


    public class ShoppingCartBuilder : IShoppingCartBuilder
    {
        private readonly IShoppingCartRepository repo;
        private readonly IMapper mapper;

        public ShoppingCartBuilder(IShoppingCartRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<ShoppingCartDTO> GetCartAsync(GetShoppingCartByCustomerRequest request)
        { 
            var cart = await repo.GetCartByCustomerAsync(request.CustomerId);

            if (cart == null)
            {
                cart = Domain.ShoppingCart.Create(request.CustomerId);

                await repo.SaveAsync(cart);
            }

            return mapper.Map<ShoppingCartDTO>(cart); 
        } 
    }
}