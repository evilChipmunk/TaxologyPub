using Shared.Domain;

namespace Order.Domain
{
    public class TaxService : ITaxService
    {
        public decimal GetTaxRate(string stateAbbreviation)
        {
            var state = States.GetByAbbreviation(stateAbbreviation);

            //obviously not valid values!!! 
            if (state.Abbreviation.Equals("AL"))
            {
                return (decimal).04;
            }
            else if (state.Abbreviation.Equals("AZ"))
            {
                return (decimal).05;
            }
            else if (state.Abbreviation.Equals("CA"))
            {
                return (decimal).06;
            }
            else if (state.Abbreviation.Equals("NY"))
            {
                return (decimal).07;
            }
            else if (state.Abbreviation.Equals("TX"))
            {
                return (decimal).08;
            }

            return (decimal).000001;

        }
    }
}