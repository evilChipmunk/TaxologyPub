using System;
using Shared.Domain;

namespace Order.Domain
{
    public class Customer : BaseEntity
    {
        public static Customer Create(Guid id, string firstName, string lastName)
        {
            Customer customer = new Customer();
            customer.Id = id;
            customer.State = SaveState.UnSaved;

            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.FullName = $"{firstName} {lastName}";

            return customer;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string FullName { get; private set; }  
    }
}