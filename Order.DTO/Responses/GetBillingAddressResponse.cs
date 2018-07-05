using Shared.DTO;

namespace Order.DTO
{
    public class GetBillingAddressResponse : BaseResponse
    {
        public GetBillingAddressResponse()
        {

        }

        public GetBillingAddressResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }

        public GetBillingAddressResponse(bool success, ResponseAction responseAction, BillingAddressDTO billingAddress) : base(success, responseAction)
        {
            BillingAddress = billingAddress;
        }

        public BillingAddressDTO BillingAddress { get; set; }
    }
}