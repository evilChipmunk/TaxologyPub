using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Mapping;
using Taxology.Site.Models;
using Taxology.Site.Services;

namespace Taxology.Site.Controllers
{
    public class OrdersController : BaseWebController
    {
        private readonly ICustomerRestService customerService;
        private readonly IOrderRestService orderService;
        private readonly IMapper mapper;

        public OrdersController(UserManager<SiteUser> userManager, 
            ICustomerRestService customerService, IOrderRestService orderService, IMapper mapper
            ) : base(userManager)
        {
            this.customerService = customerService;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetSiteUser();

            var customer = await customerService.GetCustomer(user.CustomerId);

            var orders = await orderService.GetCustomerOrders(user.CustomerId);

            var orderModels = mapper.Map<IEnumerable<OrderModel>>(orders);

            orderModels = orderModels.OrderBy(x => x.OrderDate);

            var customerModel = mapper.Map<CustomerModel>(customer);

            var model = new CustomerOrdersModel {Customer = customerModel, Orders = orderModels};

            return View(model);
        }
    }


    public class CustomerOrdersModel
    {

        public CustomerModel Customer { get; set; }
        public IEnumerable<OrderModel> Orders { get; set; }


    }
}