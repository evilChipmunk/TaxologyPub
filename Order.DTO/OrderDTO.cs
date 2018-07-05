using System;
using System.Collections.Generic;
using Shared.DTO;

namespace Order.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }

        public BillingAddressDTO BillingAddress { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO Tax { get; set; }

        public MoneyDTO Total { get; set; }
        public OrderStatus OrderStatus { get; private set; }
    }


    public enum OrderStatus
    {
        Created, Confirmed, Cancelled
    }
}