using System;

namespace Order.DTO
{
    public class OrderCustomerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}