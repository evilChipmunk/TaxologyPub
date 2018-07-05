using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class ClearCartResponse : BaseResponse
    {
        public ClearCartResponse()
        {

        }

        public ClearCartResponse(bool success, ResponseAction action) : base(success, action)
        {

        } 
    }
}