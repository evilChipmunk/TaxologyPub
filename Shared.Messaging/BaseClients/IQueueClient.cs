using System;
using System.Threading.Tasks; 
using Microsoft.Azure.ServiceBus;

namespace Subscribers
{
    public interface IQueueClient<T>
    {  
        Task Abandon(Message message);
        Task AddToDeadLetterQueue(Message message);
        Task Complete(Message message);
        Task SendMessage(T entity);

        void RegisterForMessages(Action<MessagePackage<T>> messageHandler);
    }
}