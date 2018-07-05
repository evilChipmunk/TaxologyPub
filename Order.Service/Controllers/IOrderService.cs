using System.Threading.Tasks;
using Order.DTO;

namespace Order.Service.Controllers
{
    public interface IOrderService
    {
        Task<GetBillingAddressResponse> GetMostRecentBillingAddress(GetBillingAddressRequest request);
        Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
        Task<CompleteOrderResponse> CompleteOrder(CompleteOrderRequest request);
        Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request);

        Task<GetCustomerOrdersResponse> GetCustomerOrders(GetCustomerOrdersRequest request);
        Task<GetOrderResponse> GetOpenOrder(GetOpenOrderRequest request);
    }
}