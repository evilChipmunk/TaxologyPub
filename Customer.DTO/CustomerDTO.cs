using System;

namespace Customer.DTO
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
         
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string ZipCode { get; set; }
          
        public string Country { get; set; }
        public string StateAbbreviation { get; set; }
        public Guid AnonymousId { get; set; }
    }
}
