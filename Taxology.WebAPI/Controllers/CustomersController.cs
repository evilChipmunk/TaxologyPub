using System; 
using System.Threading;
using System.Threading.Tasks; 
using Customer.DTO;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using Shared.Service; 

namespace Taxology.WebAPI.Controllers
{
    public static class Routing
    {
        public const string GetCustomer = "getcustomers";
        public const string GetCustomers = "getallcustomers";
        public const string CreateCustomer = "createcustomer";
        public const string UpdateCustomer = "updatecustomer";
        public const string DeleteCustomer = "deletecustomer";
    }


    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : BaseApiController
    {
        private readonly IRequestClientFactory getClientFactory;


        public CustomersController(ILoggerFactory loggerFactory
            , IRequestClientFactory getClientFactory

        ) : base(loggerFactory)
        {
            this.getClientFactory = getClientFactory;
        }

        protected override ILogger CreateLogger()
        {
            return this.loggerFactory.CreateLogger<CustomersController>();
        }

        [ResponseCache(Duration = 30)]
        [HttpGet("{id}", Name = Routing.GetCustomer)]
        public async Task<IActionResult> Get(Guid id)
        {
            var client = getClientFactory.Create<IGetCustomerRequest, GetCustomerResponse>();

            return await HandleAsync(async () => await client.Request(new {Id = id}, CancellationToken.None));
        }

        //        [HttpGet(Name = Routing.GetCustomers)]
        //        public async Task<IActionResult> GetAll()
        //        {
        //            return await HandleAsync(async () =>
        //              await workflow.Get(new GetAllCustomersRequest()));
        //        }
        //
        [HttpPost(Name = Routing.CreateCustomer)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            var client = getClientFactory.Create<ICreateCustomerRequest, CreateCustomerResponse>();

            return await HandleAsync(async () =>
            {
                var response = await client.Request(request, CancellationToken.None);
                string link = Url.Link(Routing.GetCustomer, new {id = response.Customer.Id});

                response.RequestUri = link;

                return response;

            });
        }

        [HttpPut(Name = Routing.UpdateCustomer)]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SaveCustomerRequest request)
        {
            var client = getClientFactory.Create<ISaveCustomerRequest, SaveCustomerResponse>();
            return await HandleAsync(async () => await client.Request(request, CancellationToken.None));
        }

    }
} 