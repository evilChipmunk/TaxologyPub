using Shared.Domain;

namespace Order.Domain
{
    public interface ITaxService : IDomainService
    {
        decimal GetTaxRate(string stateAbbreviation);
    }
}