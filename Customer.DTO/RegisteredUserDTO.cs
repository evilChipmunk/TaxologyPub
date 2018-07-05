using System;

namespace Customer.DTO
{
    public class RegisteredUserDTO
    {
        public Guid AnonymousId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}