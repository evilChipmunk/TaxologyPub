using System.Threading.Tasks;
using Order.Domain;
using Order.DTO;

namespace Order.Service.Builders
{
    public interface ICustomerBuilder
    {
        Task<Customer> Build(CreateOrderRequest request);
    }
}