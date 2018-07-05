using System.Collections.Generic;
using Shared.DTO;

namespace Order.DTO
{
    public class GetCustomerOrdersResponse : BaseResponse
    {
        public GetCustomerOrdersResponse()
        {

        }
        public GetCustomerOrdersResponse(IEnumerable<OrderDTO> orderDtOs) : base(true, ResponseAction.Found)
        {
            Orders = orderDtOs;
        }
        public GetCustomerOrdersResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {
        }

        public IEnumerable<OrderDTO> Orders { get; set; }
    }
}