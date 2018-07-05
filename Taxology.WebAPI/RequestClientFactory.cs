using System;
using MassTransit;
using Shared.Service;

namespace Taxology.WebAPI
{
    public class RequestClientFactory : IRequestClientFactory 
    {
        private readonly IBus bus;
        private readonly IRabbitMQConfiguration config;

        public RequestClientFactory(IBus bus, IRabbitMQConfiguration config)
        {
            this.bus = bus;
            this.config = config;
        }

        public IRequestClient<TRequest, TResult> Create<TRequest, TResult>() where TRequest : class where TResult : class
        {
            string name =  typeof(TRequest).Name;
            var address = new Uri($"{config.HostName}/{name}");
            var requestTimeout = TimeSpan.FromSeconds(30);

            IRequestClient<TRequest, TResult> client =
                new MessageRequestClient<TRequest, TResult>(bus, address, requestTimeout);

            return client;
        }


    }
}