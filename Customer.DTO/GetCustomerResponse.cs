using Shared.DTO;

namespace Customer.DTO
{
    public class GetCustomerResponse : BaseResponse
    {
        public GetCustomerResponse()
        {

        }
        public GetCustomerResponse(CustomerDTO customer) : base(true, ResponseAction.Found)
        {
            Customer = customer;
        }

        public GetCustomerResponse(bool success, ResponseAction action) : base(success, action)
        {

        }

        public CustomerDTO Customer { get; set; } 
    }
}