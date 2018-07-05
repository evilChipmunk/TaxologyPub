 
using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper; 
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using Product.DTO;
using Product.Persistency;
using Product.Service.Controllers;
using Shared.Persistency;
using Shared.Service;

namespace Product.Service
{
    public class ProductStartup : SharedStartup
    { 
        public ProductStartup(IConfiguration configuration, IHostingEnvironment env)
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

        private IConfiguration Configuration;
        private IDatabaseConfiguration DbConfig { get; set; }
        private IRabbitMQConfiguration RabbitConfig { get; set; }
        private IBus massBus;
         

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(RabbitConfig);
            services.AddScoped<ProductContext>();
            services.AddScoped<IProductRepository, ProductRepository>();  
             
            services.AddMediatR(typeof(ProductStartup));

            services.AddAutoMapper();
            // Add framework services. 
            var builder = new ContainerBuilder();
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

            ConfigureMassTransit(builder,
                (cfg, host) =>
                {
                    ConfigureEndpoint<ProductConsumer, GetAllProductsRequest>(host, cfg); 
                }, RabbitConfig);
   
            builder.Populate(services);
            container = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime)
        { 
            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime);


            app.EnsureMigrationOfContext<ProductContext>();
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductContext>();
//                var result = context.Database.EnsureCreatedAsync().GetAwaiter().GetResult();
//                context.Database.Migrate();
                context.EnsureSeedData().ConfigureAwait(false);
                writeToConsole(app);

            }
        }
    }
}
