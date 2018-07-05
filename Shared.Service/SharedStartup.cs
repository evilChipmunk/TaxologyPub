using System;
using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Shared.Service
{
    public  class SharedStartup
    {
        protected IContainer container;

        protected void ConfigureEndpoint<TConsumer, TRequest>(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator cfg) where TConsumer : IConsumer
        {
            string queueName = typeof(TRequest).Name;
            cfg.ReceiveEndpoint(host, queueName, e =>
            {
                e.Consumer(typeof(TConsumer), type => container.Resolve<TConsumer>());
            });
        }
        protected void ConfigureMassTransit(ContainerBuilder builder, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configureEndPoints, IRabbitMQConfiguration connectionConfig)
        { 
            builder.Register(c =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            var host = cfg.Host(new Uri(connectionConfig.HostName), h =>
                            {
                                h.Username(connectionConfig.UserName);
                                h.Password(connectionConfig.Password);
                            });

                            configureEndPoints(cfg, host);
 
                            //cfg.ReceiveEndpoint(host, "customer_update_queue", e =>
                            //{
                            //    e.Consumer<UpdateCustomerConsumer>();
                            //});
                        }
                    );
                })
                .As<IBusControl>()
                .As<IPublishEndpoint>()
                .SingleInstance();
        }
         

        protected IBusControl SetBusLifetime(IApplicationLifetime lifetime)
        {
            var bus = container.Resolve<IBusControl>();
            //start the bus
            var busHandle = TaskUtil.Await(() => bus.StartAsync());

            //register an action to call when the application is shutting down
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
            return bus;
        }


        protected void writeToConsole(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    string message = $"Message at {DateTime.Now.ToShortDateString()} \r\n";
                    message += $"Request method: {context.Request.Method} \r\n";
                    message += $"Request path: {context.Request.Path} \r\n";

                    Console.WriteLine(message);

                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                }
                await next();
            });
        }
    }
}