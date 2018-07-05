using Shared.DTO;

namespace Customer.DTO
{
    public class CreateCustomerResponse : CreatedResponse
    {
        public CreateCustomerResponse()
        {

        }

        public CreateCustomerResponse(CustomerDTO customer) 
        {
            Customer = customer;
        }

        public CreateCustomerResponse(CustomerDTO customer, string link) : base(link)
        {
            Customer = customer;
        }

        public CustomerDTO Customer { get; set; }
    }
}