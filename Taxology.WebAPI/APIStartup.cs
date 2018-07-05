using System; 
using Autofac;
using Autofac.Extensions.DependencyInjection; 
using MassTransit;  
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using Shared.Service;
using Swashbuckle.AspNetCore.Swagger; 


namespace Taxology.WebAPI
{
    public class APIStartup : SharedStartup
    { 

        public APIStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build(); 
    

            RabbitConfig = new RabbitMQConfiguration();
            Configuration.Bind("RabbitMQConfiguration", RabbitConfig);
        }

        private IConfiguration Configuration { get; }
         
        private IRabbitMQConfiguration RabbitConfig { get; set; }
        private IBus massBus;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var builder = new ContainerBuilder();

            ConfigureMassTransit(builder, (cfg, host) =>
            { 

            }, RabbitConfig);

            builder.Register(c => massBus);


            services.AddSingleton(RabbitConfig);


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



            builder.RegisterType<Taxology.WebAPI.RequestClientFactory>().As<Taxology.WebAPI.IRequestClientFactory>();
  
            builder.Populate(services);
            container = builder.Build();
             
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        } 

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime)
        {

            writeToConsole(app);



            app.UseExceptionHandler();

            app.UseErrorHandler();
            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseMvc(); 
        }

    }
}
