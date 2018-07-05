 
using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Customer.Persistency; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using AutoMapper;
using Customer.DTO;
using MassTransit; 
using MediatR; 
using Shared.Persistency;
using Shared.Service;
 
 


namespace Customer.Service
{
    public class CustomerStartup : SharedStartup
    { 
        private IBus massBus;
     
        public CustomerStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
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
                    ConfigureEndpoint<GetCustomerConsumer, IGetCustomerRequest>(host, cfg);
                    ConfigureEndpoint<CreateCustomerConsumer, ICreateCustomerRequest>(host, cfg);
                    ConfigureEndpoint<SaveCustomerConsumer, ISaveCustomerRequest>(host, cfg);
                }, RabbitConfig);


            services.AddSingleton(RabbitConfig);
            services.AddScoped<CustomerContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerBuilder, CustomerBuilder>();
    
            services.AddMediatR(typeof(CustomerStartup));
             
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

            writeToConsole(app);

            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime);


            app.EnsureMigrationOfContext<CustomerContext>();
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CustomerContext>();
//                var result = context.Database.EnsureCreatedAsync().GetAwaiter().GetResult();
//                context.Database.Migrate();
                context.EnsureSeedData().ConfigureAwait(false);
            }
        } 
    } 
}
