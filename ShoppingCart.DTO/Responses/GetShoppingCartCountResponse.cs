using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class GetShoppingCartCountResponse : BaseResponse
    {
        public GetShoppingCartCountResponse(bool success, ResponseAction responseAction, int count) : base(success, responseAction)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}