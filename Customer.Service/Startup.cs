 
using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Customer.Persistency;
using Customer.Service.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using AutoMapper;
using Customer.DTO;
using MassTransit;
using MassTransit.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Service;
using Subscribers; 
using Swashbuckle.AspNetCore.Swagger; 


namespace Customer.Service
{
    public class Startup : SharedStartup
    {
        private QueueConfig qConfig;

        private IBus massBus;
    

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
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
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddMvc();

//            services.AddScoped<CustomerContext>();
//            services.AddScoped<ICustomerService, CustomerService>();
//            services.AddScoped<ICustomerRepository, CustomerRepository>();
//            services.AddScoped<ICustomerBuilder, CustomerBuilder>();
//            services.AddScoped<CustomerSeeder>();
//            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>(); 
//            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
//            services.AddScoped<IUrlHelper>(x =>
//            {
//                var service = x.GetService<IActionContextAccessor>();
//                var actionContext = service.ActionContext;
//                return new UrlHelper(actionContext);
//            });
//            services.TryAddSingleton(qConfig);
//           // services.TryAddSingleton<CustomerCreatedTopicClient>();
//            // services.AddSingleton<ILoggerFactory>();


//            services.AddMediatR(typeof(Startup));
//         //   services.AddScoped<INotificationHandler<CustomerCreatedEvent>, CustomerCreatedTopicClient>();
//          //  services.AddScoped<IDomainNotificationHandler<CustomerCreatedAppEvent>, TestEventHandler>();
//          //  services.AddScoped<IDomainMediator, MediatRWrapper>();
////
////
////
////            System.ArgumentException: 'Cannot instantiate implementation type '
////            Customer.Service.NotificationHandlerWrapper`2[T1, T2]' for service type 
////            'MediatR.INotificationHandler`1[T1]'.'



//         //   services.ConfigureHandlers(typeof(Startup), typeof(Domain.Customer));
//         //  services.ResolveAllTypes("Taxology", "Customer.Domain", "Customer.Service");

//            services.AddAutoMapper();

//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new Info
//                {
//                    Version = "v1",
//                    Title = "My API",
//                    Description = "Customer API",
//                    TermsOfService = "None",
//                    Contact = new Contact() { Name = "The contact", Email = "someone@somewhere.com", Url = "www.taxology.com" }

//                });
//            });


//            //  services.AddScoped(_ => new CustomerContext(Configuration["Data:DefaultConnection:ConnectionString"]));
//        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var builder = new ContainerBuilder();

          //  builder.RegisterType<GetCustomerConsumer>();

          //  builder.RegisterAssemblyTypes(typeof(Startup).Assembly).As<IConsumer>();

            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

            ConfigureMassTransit(builder,
                (cfg, host) =>
                {

                  //  cfg.ReceiveEndpoint(host, "customer_update_queue", e => { e.Consumer<UpdateCustomerConsumer>(); });

                    //cfg.ReceiveEndpoint(host, "CheckOrderStatusConsumer", e => { e.Consumer<CheckOrderStatusConsumer>(); });
                    //cfg.ReceiveEndpoint(host, "IGetCustomerRequest", e =>
                    //{
                    //    e.Consumer(typeof(GetCustomerConsumer), type => container.Resolve<GetCustomerConsumer>()); 
                    //});

                    ConfigureEndpoint<GetCustomerConsumer, IGetCustomerRequest>(host, cfg);
                    ConfigureEndpoint<CreateCustomerConsumer, ICreateCustomerRequest>(host, cfg);
                    ConfigureEndpoint<SaveCustomerConsumer, ISaveCustomerRequest>(host, cfg);
                });
    
            services.AddMvc();

            services.AddScoped<CustomerContext>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerBuilder, CustomerBuilder>();
            services.AddScoped<CustomerSeeder>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var service = x.GetService<IActionContextAccessor>();
                var actionContext = service.ActionContext;
                return new UrlHelper(actionContext);
            });
            services.TryAddSingleton(qConfig); 

            services.AddMediatR(typeof(Startup));
             
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 

            //resolve the bus from the container
            massBus = SetBusLifetime(lifetime);

            app.UseMvc();
             
        } 
    }




    //public class MediatRWrapper : IDomainMediator
    //{
    //    private readonly MediatR.IMediator _mediator;

    //    public MediatRWrapper(MediatR.IMediator mediator)
    //    {
    //        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    //    }

    //    public Task Publish<TNotification>(TNotification notification,
    //        CancellationToken cancellationToken = default(CancellationToken))
    //        where TNotification : IDomainNotification
    //    {
    //        var notification2 = new NotificationWrapper<TNotification>(notification);
    //        return _mediator.Publish(notification2, cancellationToken);
    //    }
    //}

    //public class NotificationWrapper<T> : MediatR.INotification
    //{
    //    public T Notification { get; }

    //    public NotificationWrapper(T notification)
    //    {
    //        Notification = notification;
    //    }
    //}

    //public class NotificationHandlerWrapper<T1, T2> : MediatR.INotificationHandler<T1>
    //    where T1 : NotificationWrapper<T2>
    //    where T2 : IDomainNotification
    //{
    //    private readonly IEnumerable<IDomainNotificationHandler<T2>> _handlers;

    //    //the IoC should inject all domain handlers here
    //    public NotificationHandlerWrapper(
    //        IEnumerable<IDomainNotificationHandler<T2>> handlers)
    //    {
    //        _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
    //    }

    //    public Task Handle(T1 notification, CancellationToken cancellationToken)
    //    {
    //        var handlingTasks = _handlers.Select(h =>
    //            h.Handle(notification.Notification, cancellationToken));
    //        return Task.WhenAll(handlingTasks);
    //    }
    //}
}
