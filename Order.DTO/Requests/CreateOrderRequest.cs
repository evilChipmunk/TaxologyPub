using System.Collections.Generic;

namespace Order.DTO
{
    public class CreateOrderRequest
    {
        public OrderCustomerDTO Customer { get; set; }
        public BillingAddressDTO BillingAddress { get; set; }

        public IEnumerable<ProductDTO> Products { get; set; }
    }
}