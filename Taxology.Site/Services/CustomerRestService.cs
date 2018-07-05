using System;
using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Shared.Authentication; 
using Shared.Service;
using Taxology.Site.Models; 

namespace Taxology.Site.Services
{
    public interface ICustomerRestService
    {
        Task<CustomerDTO> GetCustomer(Guid customerId);
        Task<CustomerDTO> Create(RegisteredUserDTO registerUser);
        Task<CustomerDTO> SaveCustomer(PurchaseModel model, SiteUser user);
    }


    public class CustomerRestService : ICustomerRestService
    {
        private readonly IRestClient client;
        private readonly RoutingConfig routingConfig;
        private readonly IMapper mapper;

        public CustomerRestService(IRestClient client, RoutingConfig routingConfig, IMapper mapper)
        {
            this.client = client;
            this.routingConfig = routingConfig;
            this.mapper = mapper;
        }

        public async Task<CustomerDTO> GetCustomer(Guid customerId)
        {
            CustomerDTO customer = null;

            await client.Get<GetCustomerResponse>(
                routingConfig.CustomerService.URL,
                routingConfig.CustomerService.Get,
                customerId,
                success: (response) =>
                {
                    customer = response.Customer; 
                },
                error: (error) =>
                {
                    string message = error.Message;
                },
                headerAccept: HeaderAccept.Json
            );
            return customer;
        }

        public async Task<CustomerDTO> Create(RegisteredUserDTO registerUser)
        {

            CreateCustomerRequest request = new CreateCustomerRequest();
            request.RegisteredUser = registerUser;
            var response =
                await client.Post<CreateCustomerRequest, SaveCustomerResponse>(routingConfig.CustomerService.URL,
                    routingConfig.CustomerService.Create, request, HeaderAccept.Json);

            return response.Customer;
        }

        public async Task<CustomerDTO> SaveCustomer(PurchaseModel model, SiteUser user)
        {
            var request = new SaveCustomerRequest();
            var existingCustomerDTO = await GetCustomer(user.CustomerId);

            existingCustomerDTO = mapper.Map<SiteUser, CustomerDTO>(user, existingCustomerDTO);
            existingCustomerDTO = mapper.Map(model.Billing, existingCustomerDTO);
            request.Customer = existingCustomerDTO;

            var response = await client.Put<SaveCustomerRequest, SaveCustomerResponse>(routingConfig.CustomerService.URL,
                routingConfig.CustomerService.Update, request, HeaderAccept.Json);

            return response.Customer; 
        }
    }
}