 
using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using AutoMapper; 
using MassTransit; 
using MediatR; 
using Order.Domain;
using Order.DTO;
using Order.Persistency;
using Order.Service.Builders;
using Order.Service.Consumers;
using Shared.Persistency;
using Shared.Service;
 
 


namespace Order.Service
{
    public class OrderStartup : SharedStartup
    { 
        private IBus massBus;
     
        public OrderStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            DbConfig = new DatabaseConfiguration();
            Configuration.Bind("DatabaseConfiguration", DbConfig);

            RabbitConfig = new RabbitMQConfiguration();
            Configuration.Bind("RabbitMQConfiguration", RabbitConfig);
        }

        public IConfiguration Configuration { get; }
        private IDatabaseConfiguration DbConfig { get; set; }
        private IRabbitMQConfiguration RabbitConfig { get; set; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services. 
            var builder = new ContainerBuilder(); 
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

            ConfigureMassTransit(builder,
                (cfg, host) =>
                { 
                    ConfigureEndpoint<GetBillingConsumer, GetBillingAddressRequest>(host, cfg);
                    ConfigureEndpoint<GetOpenOrderConsumer, GetOpenOrderRequest>(host, cfg);
                    ConfigureEndpoint<GetOrderConsumer, GetOrderRequest>(host, cfg);
                    ConfigureEndpoint<GetCustomerOrdersConsumer, GetCustomerOrdersRequest>(host, cfg);
                    ConfigureEndpoint<CreateOrderConsumer, CreateOrderRequest>(host, cfg);
                    ConfigureEndpoint<CompleteOrderConsumer, CompleteOrderRequest>(host, cfg);
                    ConfigureEndpoint<CancelOrderConsumer, CancelOrderRequest>(host, cfg); 
                }, RabbitConfig);


            services.AddSingleton(RabbitConfig);
            services.AddScoped<OrderContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<IOrderBuilder, OrderBuilder>();
            services.AddScoped<ICustomerBuilder, CustomerBuilder>();
        
 

            services.AddMediatR(typeof(OrderStartup));
             
            services.AddAutoMapper();

            builder.Populate(services);
            container = builder.Build();
             
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }
         
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime)
        {
            app.EnsureMigrationOfContext<OrderContext>();
             
            writeToConsole(app);

            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime); 
        } 
    } 
}
