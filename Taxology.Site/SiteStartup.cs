 
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using Shared.Authentication;
using Shared.Persistency;
using Shared.Service;
using Taxology.Site.Controllers;
using Taxology.Site.Services; 

namespace Taxology.Site
{


    public class SiteStartup : SharedStartup
    {
        public SiteStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
             

            RoutingConfiguration = new RoutingConfig();
            Configuration.Bind("RoutingConfig", RoutingConfiguration);
            //set all URLS to point to web api -- doesn't need to be separate here even if configuration allows it
            RoutingConfiguration.CustomerService.URL = RoutingConfiguration.URL;
            RoutingConfiguration.OrderService.URL = RoutingConfiguration.URL;
            RoutingConfiguration.ProductService.URL = RoutingConfiguration.URL;
            RoutingConfiguration.ShoppingCartService.URL = RoutingConfiguration.URL;
            RoutingConfiguration.PurchasedProductService.URL = RoutingConfiguration.URL;

            DbConfig = new DatabaseConfiguration();
            Configuration.Bind("DatabaseConfiguration", DbConfig);

            RabbitConfig = new RabbitMQConfiguration();
            Configuration.Bind("RabbitMQConfiguration", RabbitConfig); 

        }

        private IConfiguration Configuration { get; }
        private RoutingConfig RoutingConfiguration { get; }
        private IDatabaseConfiguration DbConfig { get; set; }
        private IRabbitMQConfiguration RabbitConfig { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRestClient, RestClient>();
            services.TryAddSingleton(RoutingConfiguration);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>(); 
            //services.AddDbContext<SiteUserContext>(options => options.UseSqlServer(DbConfig.ConnectionString,
            //     o => o.EnableRetryOnFailure()));
            services.AddScoped<SiteUserContext>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<AnoymousIdHandler>();

            services.AddTransient<ICustomerRestService, CustomerRestService>();
            services.AddTransient<IOrderRestService, OrderRestService>();
             


            ConfigureAuth(services);

            services.AddAutoMapper();

            services.AddSession();
            services.AddMvc();
        }

        public void ConfigureAuth(IServiceCollection services)
        {
            services.AddIdentity<SiteUser, IdentityRole>(config =>
                {
                    // If you change this, you need to change the regular expression in the Vue code too!
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireLowercase = true;
                    config.Password.RequireUppercase = true;
                    config.Password.RequireNonAlphanumeric = false;
                    config.User.RequireUniqueEmail = true;
                    config.SignIn.RequireConfirmedEmail = false;
                    config.Lockout.MaxFailedAccessAttempts = 1000;

                })
                .AddEntityFrameworkStores<SiteUserContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(config =>
                {
                    config.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (ctx) =>
                        {

                            string returnURL = ctx.Request.GetEncodedPathAndQuery();
                            ctx.Response.Redirect("/account/login?returnURL=" + returnURL);
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 403;
                            }

                            return Task.CompletedTask;
                        }

                    };
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            writeToConsole(app);


            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            app.UseAuthentication();
          
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseErrorHandler();
            //

            app.EnsureMigrationOfContext(new SiteUserContext());
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetService<SiteUserContext>();
            //   // var result = context.Database.EnsureCreatedAsync().GetAwaiter().GetResult();

            //    context.Database.Migrate();
            //   // context.Database.Migrate(); 
            //}


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
         
    }  
}
