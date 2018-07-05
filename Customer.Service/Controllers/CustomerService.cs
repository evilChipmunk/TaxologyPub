using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Customer.Service.Controllers
{

    public interface ICustomerService
    {
        Task<GetCustomerResponse> Get(IGetCustomerRequest request);
        Task<GetAllCustomersResponse> Get(GetAllCustomersRequest request);

        Task<CreateCustomerResponse> Create(CreateCustomerRequest request);

        Task<SaveCustomerResponse> Update(SaveCustomerRequest request);

        Task<DeleteResponse> Delete(DeleteRequest request);
    }

    public class CustomerService : ICustomerService
    {
        private ICustomerRepository repo;
        private readonly IMapper mapper;
        private readonly ICustomerBuilder builder;
        private readonly IUrlHelper urlHelper;

        public CustomerService(ICustomerRepository repo, IMapper mapper, ICustomerBuilder builder, IUrlHelper urlHelper)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.builder = builder;
            this.urlHelper = urlHelper;
            this.mapper = mapper;
        }

        public async Task<GetCustomerResponse> Get(IGetCustomerRequest request)
        {
            var customer = await repo.GetCustomerByIdAsync(request.Id);
            if (customer == null)
            {
                return new GetCustomerResponse(false, ResponseAction.NotFound);
            }  

            var dto = mapper.Map<CustomerDTO>(customer);
            return new GetCustomerResponse(dto);  
        }

        public async Task<GetAllCustomersResponse> Get(GetAllCustomersRequest request)
        {
            var customers = await repo.GetCustomersAsync();

            var dtos = mapper.Map<IEnumerable<CustomerDTO>>(customers);
            var response = mapper.Map<GetAllCustomersResponse>(dtos);
            response.ResponseAction = ResponseAction.Found;
            return response;
        }


        public async Task<CreateCustomerResponse> Create(CreateCustomerRequest request)
        {
            var customer = builder.Build(request.RegisteredUser);
            customer = await this.repo.SaveAsync(customer);
            var dto = mapper.Map<CustomerDTO>(customer);

            string link = urlHelper.Link(Routing.GetCustomer, new { id = customer.Id });
            var response = new CreateCustomerResponse(dto, link); 
            return response;
        }

        public async Task<SaveCustomerResponse> Update(SaveCustomerRequest request)
        {
            var customer = builder.Build(request.Customer);

            customer.UpdateAddress(request.Customer.Address1, request.Customer.Address2, request.Customer.ZipCode,
                request.Customer.Country, request.Customer.StateAbbreviation);

            customer = await this.repo.SaveAsync(customer);
            var dto = mapper.Map<CustomerDTO>(customer);


            var response = mapper.Map<SaveCustomerResponse>(dto);
            response.ResponseAction = ResponseAction.Updated;
            return response;
        }

        public async Task<DeleteResponse> Delete(DeleteRequest request)
        {
            repo.Delete(request.Id);
            return new DeleteResponse(true, ResponseAction.Deleted);  
        }
    }
}