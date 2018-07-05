using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Customer.DTO;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistency;
using Shared.Service;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;
using ShoppingCart.Service.Consumers;
using ShoppingCart.Service.Controllers;

namespace ShoppingCart.Service
{
    public class ShoppingStartup : SharedStartup
    { 
        private IBus massBus;
     
        public ShoppingStartup(IConfiguration configuration, IHostingEnvironment env)
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

        private IConfiguration Configuration { get; }
        private IDatabaseConfiguration DbConfig { get; set; }
        private IRabbitMQConfiguration RabbitConfig { get; set; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services. 
            var builder = new ContainerBuilder(); 
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

            services.AddSingleton(RabbitConfig);
            services.AddScoped<IShoppingCartBuilder, ShoppingCartBuilder>();

            ConfigureMassTransit(builder,
                (cfg, host) =>
                { 
                    ConfigureEndpoint<AddProductConsumer, AddProductRequest>(host, cfg);
                    ConfigureEndpoint<RemoveProductConsumer, RemoveProductRequest>(host, cfg);
                    ConfigureEndpoint<ClearCartConsumer, ClearCartRequest>(host, cfg);
                   // ConfigureEndpoint<CreateShoppingCartConsumer, CreateShoppingCartRequest>(host, cfg);
                    ConfigureEndpoint<GetShoppingCartByCustomerConsumer, GetShoppingCartByCustomerRequest>(host, cfg);

                    string queueName = typeof(ICustomerCreatedNotification).Name;
                    cfg.ReceiveEndpoint(host, queueName, e =>
                    {
                        e.UseRetry(r => r.Immediate(5));
                        e.Consumer(typeof(CustomerCreatedConsumer), type => container.Resolve<CustomerCreatedConsumer>());
                    });
                }, RabbitConfig);


            services.AddScoped<ShoppingCartContext>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
  

            services.AddMediatR(typeof(ShoppingStartup));
             
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

            app.EnsureMigrationOfContext<ShoppingCartContext>();
            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime);
            writeToConsole(app);

        }
    } 
}
