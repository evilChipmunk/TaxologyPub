using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Customer.Domain;
using Customer.DTO;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Shared.Domain;
using Subscribers;

namespace Customer.Service.Messaging
{


    public class CustomerCreatedTopicClient : BaseTopicClient<CustomerDTO>
       // , IHandle<CustomerCreatedEvent>
        , INotificationHandler<CustomerCreatedEvent>
    {
        private readonly IMapper mapper;

        public CustomerCreatedTopicClient(QueueConfig config, IMapper mapper) : base("customercreated", config.ClientConnection)
        {
            this.mapper = mapper;
        }
        public override async Task SendMessage(CustomerDTO doc)
        {
           // var topicClient = (ITopicClient)sender; 
            var message = GetBasicMessage(doc);
           // message.UserProperties.Add(Configuration.TopicDocumentFilter, doc.SubmissionId.ToString());

            await base.SendMessage(message);
        }
//
//        public void Handle(CustomerCreatedEvent args)
//        {
//            SendMessage(args.Customer).ConfigureAwait(false);
//        }

        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var customerDTO = mapper.Map<CustomerDTO>(notification.Customer);
           await  SendMessage(customerDTO);
        }
    }



//    public class TestEventHandler : IDomainNotificationHandler<CustomerCreatedAppEvent>
//    {
//        public Task Handle(CustomerCreatedAppEvent notification, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            int i = 0;
//            int a = i;
//            return Task.CompletedTask;
//        }
//    }


    public class CustomerCreatedAppEvent : IDomainEvent 
    {
        public CustomerCreatedAppEvent(CustomerDTO customer)
        {
            DateTimeEventOccurred = DateTime.Now;
            EventType = "customercreated";
            Customer = customer;
        }
        public DateTime DateTimeEventOccurred { get; }
        public string EventType { get; }
        public CustomerDTO Customer { get; }
    }


    //public class CompletedTransformTopicClient : BaseTopicClient<Document>
    //{
    //    public CompletedTransformTopicClient() : base(Configuration.CompletedTransforms)
    //    {
    //    }
    //    public override async Task SendMessage(Document doc)
    //    {
    //        var topicClient = (TopicClient)sender; 
    //        var message = GetBasicMessage(doc);
    //        message.UserProperties.Add(Configuration.TopicDocumentFilter, doc.SubmissionId.ToString());

    //        await base.SendMessage(message);
    //    }
    //} 
}
