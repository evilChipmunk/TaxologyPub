 
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Persistency;
using Product.Service.Controllers;
using Swashbuckle.AspNetCore.Swagger;

namespace Product.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ProductContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ProductSeeder>();


            services.AddMediatR(typeof(Startup));

            services.AddAutoMapper();

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
                var seeder = serviceScope.ServiceProvider.GetService<ProductSeeder>();
                seeder.SeedData().GetAwaiter().GetResult();
            }
             

            app.UseMvc();
        }
    }
}
