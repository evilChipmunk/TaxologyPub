using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscribers
{ 
    public abstract class BaseServiceBusClient<T>
    {
        protected BaseServiceBusClient(IReceiverClient receiver, ISenderClient sender)
        { 
            this.receiver = receiver;
            this.sender = sender;
        }

        protected readonly IReceiverClient receiver;
        protected readonly ISenderClient sender;

        protected Message GetBasicMessage(T entity)
        {
            var message = new Message();
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            message.Body = Encoding.UTF8.GetBytes(jsonData);
            return message;
        }

        public virtual async Task SendMessage(T entity)
        {
            var message = GetBasicMessage(entity);
            await sender.SendAsync(message);
        }

        protected async Task SendMessage(Message message)
        {
            await sender.SendAsync(message);
        }

        protected EventHandler<Message> MessageReceived { get; set; }

        public virtual async Task AddToDeadLetterQueue(Message message)
        {
            await receiver.DeadLetterAsync(message.SystemProperties.LockToken);
        }

        public virtual async Task Complete(Message message)
        {
            await receiver.CompleteAsync(message.SystemProperties.LockToken);
        }

        public virtual async Task Abandon(Message message)
        {
            await receiver.AbandonAsync(message.SystemProperties.LockToken);
        }

        private async Task MessageException(ExceptionReceivedEventArgs arg)
        {
            System.Diagnostics.Trace.TraceError(arg.Exception.Message);
        }

        private async Task MessageHandler(Message message, CancellationToken arg2)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, message);
            }
            else
            {
                throw new Exception("Handler was not hooked up!");
            }
        }

        private void RegisterForMessages()
        {
            var options = new MessageHandlerOptions(MessageException);
            options.AutoComplete = false;
            options.MaxConcurrentCalls = int.MaxValue;
            receiver.RegisterMessageHandler(MessageHandler, options);
        }

        public void RegisterForMessages(Action<MessagePackage<T>> messageHandler)
        {
            this.MessageReceived += new EventHandler<Message>((obj, message) => {

                var mp = new MessagePackage<T>();
                mp.Message = message;
                mp.Entity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                messageHandler(mp);
            });
            RegisterForMessages();
        }

         
    } 
}
