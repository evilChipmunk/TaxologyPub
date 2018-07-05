using System;
using System.Threading.Tasks;
using Customer.Domain;
using Customer.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTO;
using Shared.Service;

namespace Customer.Service.Controllers
{

    public static class Routing
    {
        public const string GetCustomer = "Get";
        public const string GetCustomers = "GetAll";
        public const string CreateCustomer = "create";
        public const string UpdateCustomer = "Update";
        public const string DeleteCustomer = "Delete";
    }

    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService workflow;
   
        public CustomersController(ICustomerService workflow, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.workflow = workflow; 

        }
        protected override ILogger CreateLogger()
        {
            return this.loggerFactory.CreateLogger<CustomersController>();
        }

//        [HttpGet("{id}", Name = Routing.GetCustomer)] 
//        public async Task<IActionResult> Get(Guid id)
//        {
//            return await HandleAsync(async () =>
//            { 
//                return await workflow.Get(new GetCustomerRequest(id)); 
//            });
//        }

        [HttpGet(Name = Routing.GetCustomers)]
        public async Task<IActionResult> GetAll()
        {
            return await HandleAsync(async () =>
              await workflow.Get(new GetAllCustomersRequest()));
        }

        [HttpPost(Name = Routing.CreateCustomer)]
        [Route(Routing.CreateCustomer)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            return await HandleAsync(async () =>
            {
                var response = await workflow.Create(request);
                 
                return response;

            });
        }

        [HttpPut(Name = Routing.UpdateCustomer)]
        [Route(Routing.UpdateCustomer)]
        public async Task<IActionResult> Update([FromBody] SaveCustomerRequest request)
        {
            return await HandleAsync(async () => await this.workflow.Update(request));
        }

        [HttpDelete(Name = Routing.DeleteCustomer)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await HandleAsync(async () => await this.workflow.Delete(new DeleteRequest(id)));
        }
    }


}