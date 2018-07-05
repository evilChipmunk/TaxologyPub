using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Order.DTO;
using Shared.Service;
using ShoppingCart.DTO;
using Taxology.Site.Models;

namespace Taxology.Site.Services
{
    public interface IOrderRestService
    {
        Task<BillingAddressDTO> GetDefaultBilling(Guid customerId);
        Task<OrderDTO> CreateOrder(Customer.DTO.CustomerDTO customer, ShoppingCartDTO cart, BillingAddressDTO billing);
        Task<bool> CompleteOrder(Guid orderId);
        Task<OrderDTO> GetOpenOrderForCustomer(Guid customerId);
        Task<IEnumerable<OrderDTO>> GetCustomerOrders(Guid customerId);
        Task<OrderDTO> GetOrder(Guid orderId);
    }

    public class OrderRestService : IOrderRestService
    {
        private readonly IRestClient client;
        private readonly OrderServiceRoutingConfig routingConfig;
        private readonly IMapper mapper;

        public OrderRestService(IRestClient client, RoutingConfig routingConfig, IMapper mapper)
        {
            this.client = client;
            this.routingConfig = routingConfig.OrderService;
            this.mapper = mapper;
        }
         
        public async Task<BillingAddressDTO> GetDefaultBilling(Guid customerId)
        {
            BillingAddressDTO billing = null;
      
            await client.Get<GetBillingAddressResponse>(routingConfig.URL,
                routingConfig.GetBillingAddress, customerId,
                success: (response) => { billing = response.BillingAddress; },
                error: (error) =>
                {
                    if (error.Response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //no problem
                    }
                },
                headerAccept: HeaderAccept.Json);

            return billing;
        }
         
        public async Task<OrderDTO> CreateOrder(Customer.DTO.CustomerDTO customer, ShoppingCartDTO cart, BillingAddressDTO billing)
        {
            var request = new CreateOrderRequest();
             
            request.Customer = mapper.Map<OrderCustomerDTO>(customer);
            request.BillingAddress = billing; 
            request.Products = mapper.Map<IEnumerable<ProductDTO>>(cart.Products);

            var response = await client.Post<CreateOrderRequest, CreateOrderResponse>(
                routingConfig.URL,
                routingConfig.CreateOrder, request, HeaderAccept.Json);

            return response.Order;
        }
         
        public async Task<bool> CompleteOrder(Guid orderId)
        {
            var request = new CompleteOrderRequest();
            request.OrderId = orderId;


            var response = await client.Post<CompleteOrderRequest, CompleteOrderResponse>(routingConfig.URL,
                routingConfig.CompleteOrder, request, HeaderAccept.Json);

            return response.IsSuccess;
        }


        public async Task<OrderDTO> GetOpenOrderForCustomer(Guid customerId)
        {
            OrderDTO order = null;
               
            await client.Get<GetOpenOrderResponse>(routingConfig.URL, routingConfig.GetOpenOrder,
                customerId,
                success: (response) =>
                {
                    order = response.Order;
                },
                error: (error) =>
                {

                },
                headerAccept: HeaderAccept.Json
            );

            return order;
        }

        public async Task<IEnumerable<OrderDTO>> GetCustomerOrders(Guid customerId)
        {
            IEnumerable < OrderDTO > orders = null;
             
            await client.Get<GetCustomerOrdersResponse>(routingConfig.URL, routingConfig.GetCustomerOrders,
                customerId,
                success: (response) =>
                {
                    orders = response.Orders;
                },
                error: (error) =>
                {

                },
                headerAccept: HeaderAccept.Json
            );

            return orders;
        }

        public async Task<OrderDTO> GetOrder(Guid orderId)
        {
            OrderDTO order = null;

            await client.Get<GetOrderResponse>(routingConfig.URL, routingConfig.GetOrder,
                orderId,
                success: (response) =>
                {
                    order = response.Order;
                },
                error: (error) =>
                {

                },
                headerAccept: HeaderAccept.Json
            );

            return order;
        }

        public async Task<bool> CancelOrder(Guid orderId)
        {
            var request = new CancelOrderRequest();
            request.OrderId = orderId;


            var response = await client.Post<CancelOrderRequest, CancelOrderResponse>(routingConfig.URL,
                routingConfig.CompleteOrder, request, HeaderAccept.Json);

            return response.IsSuccess;
        }

    }
}