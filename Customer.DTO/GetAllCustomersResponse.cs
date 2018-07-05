using System.Collections.Generic;
using Shared.DTO;

namespace Customer.DTO
{
    public class GetAllCustomersResponse : BaseResponse
    {
        public GetAllCustomersResponse(bool success, ResponseAction responseAction, IEnumerable<CustomerDTO> customers) : base(success, responseAction)
        {
            Customers = customers;
        }

        public IEnumerable<CustomerDTO> Customers { get; set; }
    }
}