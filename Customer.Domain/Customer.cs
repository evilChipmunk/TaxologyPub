using Shared.Domain;
using System; 

namespace Customer.Domain
{ 
    public class Customer: BaseEntity
    {
        private Customer()
        {

        }
 
        public static Customer Create (string firstname, string lastName, string email, 
            Guid anonymousId) 
        {
            Guard.ForNullOrEmpty(firstname, "firstname");
            Guard.ForNullOrEmpty(lastName, "lastName");
            Guard.ForNullOrEmpty(email, "email"); 
            Guard.ForNullOrEmpty(anonymousId, "anonymousId");

             
            Customer customer = new Customer();
            customer.Id = Guid.NewGuid();
            customer.State = SaveState.UnSaved;
            customer.FirstName = firstname;
            customer.LastName = lastName;
            customer.Email = email;  
            customer.AnonymousId = anonymousId;
             
            customer.AddEvent(new CustomerCreatedEvent(customer));
              
            return customer;
        }

        public static Customer CreateExisting (Guid id, string firstname, string lastName, string email, string phone, string stateAbbreviation)
        {
            Guard.ForNullOrEmpty(id, "id");
            Guard.ForNullOrEmpty(firstname, "firstname");
            Guard.ForNullOrEmpty(lastName, "lastName");
            Guard.ForNullOrEmpty(email, "email");

            Guard.ForNullOrEmpty(stateAbbreviation, "stateAbbreviation");
            var foundState = States.GetByAbbreviation(stateAbbreviation);
            Guard.ForNull(foundState, "stateAbbreviation", "Invalid state abbreviation");

            var customer = new Customer();
            customer.Id = id;
            customer.FirstName = firstname;
            customer.LastName = lastName;
            customer.Email = email;
            customer.Phone = phone;
            customer.StateAbbreviation = stateAbbreviation;

            customer.AddEvent(new CustomerHydratedEvent(customer));
            return customer; 
        }

        public static Customer CreateForDelete(Guid id)
        {
            var deleted = new Customer();
            deleted.Id = id;
            return deleted;
        }

        public void UpdateAddress(string address1, string address2, string zipCode, string country,
            string stateAbbreviation)
        {
            Guard.ForNullOrEmpty(stateAbbreviation, "stateAbbreviation");
            var foundState = States.GetByAbbreviation(stateAbbreviation);
            Guard.ForNull(foundState, "stateAbbreviation", "Invalid state abbreviation");

            Address1 = address1;
            Address2 = address2;
            ZipCode = zipCode;
            Country = country;
            ResidencyState = foundState;

            AddEvent(new CustomerUpdatedEvent(this));
        }

        public Guid AnonymousId { get; set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public State ResidencyState { get; private set; }
         
        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string ZipCode { get; private set; }
          
        public string Country { get; set; }

        public string StateAbbreviation {
            get { return ResidencyState?.Abbreviation; }
            private set { ResidencyState = States.GetByAbbreviation(value); } }

    } 
}
