using System.Threading.Tasks;
using AutoMapper;
using Shared.Domain;
using Shared.DTO;
using ShoppingCart.Domain;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;

namespace ShoppingCart.Service
{

    public interface IShoppingCartService
    {
        Task<GetShoppingCartResponse> GetCartAsync(GetShoppingCartByCustomerRequest request);
       // Task<CreateShoppingCartResponse> CreateCartAsync(CreateShoppingCartRequest request);
        Task<AddProductResponse> AddProduct(AddProductRequest request);
        Task<RemoveProductResponse> RemoveProduct(RemoveProductRequest request);
        Task<ClearCartResponse> ClearCart(ClearCartRequest request);
    }

    public class ShoppingCartService : IShoppingCartService
    { 
        private readonly IShoppingCartRepository repo;
        private readonly IMapper mapper;

        public ShoppingCartService(  IShoppingCartRepository repo, IMapper mapper)
        { 
            this.repo = repo;
            this.mapper = mapper;
        }
  

        public async Task<GetShoppingCartResponse> GetCartAsync(GetShoppingCartByCustomerRequest request)
        {

            var cart = await repo.GetCartByCustomerAsync(request.CustomerId);

            if (cart == null)
            {
                cart = Domain.ShoppingCart.Create(request.CustomerId);

                await repo.SaveAsync(cart);
            }

            return GetCartDTO(cart); 
        }
//
//        public async Task<CreateShoppingCartResponse> CreateCartAsync(CreateShoppingCartRequest request)
//        {
//            var cart = Domain.ShoppingCart.Create(request.CustomerId);
//            await repo.SaveAsync(cart);
//
//            var cartDTO = mapper.Map<ShoppingCartDTO>(cart);
//
//            return new CreateShoppingCartResponse {ShoppingCart = cartDTO};
//        }

        public async Task<AddProductResponse> AddProduct(AddProductRequest request)
        {
            var cart = await repo.GetCartByIdAsync(request.CartId);
            var product = request.Product;

            //double dispatch pattern
            cart.AddProduct(ShoppingCartProduct.Create(product.Id, product.Name, cart, new Money(product.Price.Amount)));

            await repo.SaveAsync(cart);

            return new AddProductResponse(true, ResponseAction.Updated);
        }

        public async Task<RemoveProductResponse> RemoveProduct(RemoveProductRequest request)
        {
             var cart = await repo.GetCartByIdAsync(request.CartId);
            cart.RemoveProduct(request.ProductId);

            await repo.SaveAsync(cart);

            return new RemoveProductResponse(true, ResponseAction.Deleted);
        }

        public async Task<ClearCartResponse> ClearCart(ClearCartRequest request)
        {
            var cart = await repo.GetCartByIdAsync(request.CartId);
            cart.UpdateOrder(request.OrderId);
            await repo.SaveAsync(cart);

            return new ClearCartResponse(true, ResponseAction.Updated);
        }


        private GetShoppingCartResponse GetCartDTO(Domain.ShoppingCart cart)
        {
            if (cart == null)
            {
                return new GetShoppingCartResponse(true, ResponseAction.NotFound);
            }
            var cartDTO = mapper.Map<ShoppingCartDTO>(cart);
            var response = new GetShoppingCartResponse(cartDTO, true, ResponseAction.Found);
            response.ResponseAction = ResponseAction.Found;
            return response;
        }
    }
}