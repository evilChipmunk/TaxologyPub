using System;
using Shared.Domain;

namespace Order.Domain
{
    public class BillingAddress : BaseEntity
    { 

        public string Phone { get; private set; }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string ZipCode { get; private set; }

        public string StateAbbreviation { get; private set; }

        public string Country { get; private set; }


        public Guid OrderId { get; private set; }

        public static BillingAddress Create(Order order,  string dtoAddress1, string dtoAddress2, string dtoCountry
            , string dtoPhone, string dtoStateAbbreviation, string dtoZipCode)
        {

            //add guards

            BillingAddress ba = new BillingAddress();
            ba.Id = Guid.NewGuid();
            ba.State = SaveState.UnSaved;

            ba.Address1 = dtoAddress1;
            ba.Address2 = dtoAddress2;
            ba.Country = dtoCountry;
            ba.Phone = dtoPhone;
            ba.StateAbbreviation = dtoStateAbbreviation;
            ba.ZipCode = dtoZipCode;

            ba.OrderId = order.Id;

            ba.AddEvent(new BillingAddressCreatedEvent(ba));

            return ba;

        }
    }
}