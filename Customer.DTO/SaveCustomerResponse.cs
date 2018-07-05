using Shared.DTO;

namespace Customer.DTO
{
    public class SaveCustomerResponse : BaseResponse
    {
        public SaveCustomerResponse()
        {

        }
        public SaveCustomerResponse(bool success, ResponseAction responseAction, CustomerDTO customer) : base(success, responseAction)
        {
            Customer = customer;
        }

        public CustomerDTO Customer { get; set; }
    }
}