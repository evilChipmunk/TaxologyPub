 
using Microsoft.Azure.ServiceBus;

namespace Subscribers
{
    public abstract class BaseTopicClient<T> : BaseServiceBusClient<T>
    {
        protected BaseTopicClient(string topic, string queueConnection) : base(null, new TopicClient(queueConnection, topic))
        {
        }
    } 
}
