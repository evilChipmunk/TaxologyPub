using Microsoft.Azure.ServiceBus;

namespace Subscribers
{
    public class MessagePackage<T>
    {
        public T Entity { get; set; }
        public Message Message { get; set; }
    } 
}
