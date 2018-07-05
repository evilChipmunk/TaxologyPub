using System; 
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using ShoppingCart.Persistency;
using ShoppingCart.Service.Controllers;
using Subscribers;
using Swashbuckle.AspNetCore.Swagger;

namespace ShoppingCart.Service
{

    public class Startup
    {
        private QueueConfig qConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.qConfig = new QueueConfig();
            Configuration.Bind("Queue", qConfig);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ShoppingCartContext>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>(); 
            services.AddScoped<IShoppingCartService, ShoppingCartService>();


            services.AddScoped<Func<IShoppingCartRepository>>(c => c.GetService<IShoppingCartRepository>);
            services.AddScoped<Func<IShoppingCartService>>(c => c.GetService<IShoppingCartService>);


            services.TryAddSingleton(qConfig);


           // services.AddScoped<CustomerCreatedTopicClient>();
            services.AddScoped<CustomerCreatedSubscription>();

            services.AddMediatR(typeof(Startup));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "Customer API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "The contact", Email = "someone@somewhere.com", Url = "www.taxology.com" }

                });
            });

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                createdTopicListener = serviceScope.ServiceProvider.GetService<CustomerCreatedSubscription>(); 
            }



            app.UseMvc();
        }

        private static CustomerCreatedSubscription createdTopicListener;
    }
}
