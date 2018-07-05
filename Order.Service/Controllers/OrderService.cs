using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Order.Domain;
using Order.DTO;
using Order.Persistency;
using Shared.DTO;

namespace Order.Service.Controllers
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repo;
        private readonly ICustomerRepository custRepo;
        private readonly IMapper mapper;
        private readonly ITaxService taxService;
        private readonly IUrlHelper urlHelper;

        public OrderService(IOrderRepository repo, ICustomerRepository custRepo, IMapper mapper,
            ITaxService taxService, IUrlHelper urlHelper)
        {
            this.repo = repo;
            this.custRepo = custRepo;
            this.mapper = mapper;
            this.taxService = taxService;
            this.urlHelper = urlHelper;
        }
        public async Task<GetBillingAddressResponse> GetMostRecentBillingAddress(GetBillingAddressRequest request)
        {
            var order = await repo.GetMostRecentOrderByCustomer(request.CustomerId);

            BillingAddressDTO billingAddressDTO = null;
            if (order != null)
            {
                billingAddressDTO = mapper.Map<BillingAddressDTO>(order.BillingAddress);
            }

            if (billingAddressDTO == null)
            {
                return new GetBillingAddressResponse(true, ResponseAction.NotFound);
            }

            return new GetBillingAddressResponse(true, ResponseAction.Found, billingAddressDTO); 
        }

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
        {  
            var customer = await EnsureCustomer(request);

            var order = BuildOrder(customer, request.BillingAddress, request.Products); 
            order = await repo.SaveAsync(order); 

            var orderDTO = mapper.Map<OrderDTO>(order); 
            string link = urlHelper.Link(Routing.Get, new { id = order.Id });
         

            return new CreateOrderResponse(orderDTO, link);
        }

        private async Task<Customer> EnsureCustomer(CreateOrderRequest request)
        {
            var customer = await custRepo.GetCustomer(request.Customer.Id);

            if (customer == null)
            {
                customer = Customer.Create(request.Customer.Id, request.Customer.FirstName, request.Customer.LastName);
                customer = await custRepo.SaveAsync(customer);
            }

            return customer;
        }

        private Domain.Order BuildOrder(Customer customer, BillingAddressDTO baDTO, IEnumerable<ProductDTO> productDTOs)
        {
            var taxRate = taxService.GetTaxRate(baDTO.StateAbbreviation);

            Domain.Order order = Domain.Order.Create(customer, 
                baDTO.Address1, baDTO.Address2, baDTO.Country, baDTO.Phone, baDTO.StateAbbreviation, baDTO.ZipCode, 
                taxRate);

            foreach (var product in productDTOs)
            {
                order.AddProduct(product.Id, product.Name, product.Price.Amount);
            } 
            return order;
        }
  
        public async Task<CompleteOrderResponse> CompleteOrder(CompleteOrderRequest request)
        {
            Domain.Order order = await repo.GetOrder(request.OrderId);
            order.Confirm();
            order = await repo.SaveAsync(order);

            return new CompleteOrderResponse(true, ResponseAction.Updated);
        }

        public async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request)
        {
            Domain.Order order = await repo.GetOrder(request.OrderId);
            order.Cancel();
            order = await repo.SaveAsync(order);

            return new CancelOrderResponse(true, ResponseAction.Updated);
        }

        public async Task<GetCustomerOrdersResponse> GetCustomerOrders(GetCustomerOrdersRequest request)
        {
            var orders = await repo.GetCustomerOrders(request.CustomerId);

            var orderDTOs = mapper.Map<IEnumerable<OrderDTO>>(orders);

            return new GetCustomerOrdersResponse(orderDTOs);
        }
        public async Task<GetOrderResponse> GetOpenOrder(GetOpenOrderRequest request)
        {
            var order = await repo.GetOpenOrderByCustomer(request.CustomerId);

            var orderDTO = mapper.Map<OrderDTO>(order);

            return new GetOrderResponse(orderDTO);
        }
    }
}