using Microsoft.Azure.ServiceBus;

namespace Subscribers
{
    public abstract class BaseSubscriptionClient<T> : BaseServiceBusClient<T>
    {
        protected readonly string subscriptionName;
        protected readonly string topic;
        protected BaseSubscriptionClient(ISubscriptionClient receiver) : base(receiver, null)
        {
        }


        protected BaseSubscriptionClient(QueueConfig config, string topic, string subscription):
            base(new SubscriptionClient(config.ClientConnection, topic, subscription), null)
        {
            this.subscriptionName = subscription;
            this.topic = topic;
        }
    } 
}
