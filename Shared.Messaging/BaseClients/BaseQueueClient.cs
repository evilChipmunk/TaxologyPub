 
using Microsoft.Azure.ServiceBus;

namespace Subscribers
{
    public abstract class BaseQueueClient<T> : BaseServiceBusClient<T>
    {
        protected BaseQueueClient(string queueName, string queueConnection) 
            : base(new QueueClient(queueConnection, queueName)
                  , new QueueClient(queueConnection, queueName, ReceiveMode.PeekLock))
        { 
        } 
    } 
}
