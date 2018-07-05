using System;
using System.Threading.Tasks;
using Customer.DTO;
using ShoppingCart.DTO;
using ShoppingCart.Persistency;
using Subscribers;

namespace ShoppingCart.Service.Controllers
{ 

    public class CustomerCreatedSubscription : BaseSubscriptionClient<CustomerDTO>
    {
        private readonly Func<IShoppingCartService> serviceFunc;
        private readonly Func<IShoppingCartRepository> repoFunc;

        public CustomerCreatedSubscription(QueueConfig config, Func<IShoppingCartService> service, Func<IShoppingCartRepository> repo)
            : base(config, "customercreated", "customercreatedsubscription")
        {
            this.serviceFunc = service;
            this.repoFunc = repo;
            RegisterForMessages(Handle);
        }


        private async void Handle(MessagePackage<CustomerDTO> message)
        {
            var customer = message.Entity;

            try
            {  
                var service = serviceFunc();
                var repo = repoFunc();

                var request = new GetShoppingCartByCustomerRequest(customer.AnonymousId);
               
                var response = await service.GetCartAsync(request);
                var cart = await repo.GetCartByIdAsync(response.ShoppingCart.Id);
                cart.UpdateCustomer(customer.AnonymousId, customer.Id);

                await repo.SaveAsync(cart);
                //TODO change the order after debugging is completed
                await this.Complete(message.Message);

            }
            catch (Exception ex)
            {
                await base.Abandon(message.Message);
            }

        }
    }



//
//    public class CustomerCreatedTopicClient : BaseTopicClient<CustomerDTO>
//    {
//        private readonly IShoppingCartRepository repo;
//
//        public CustomerCreatedTopicClient(QueueConfig config, IShoppingCartRepository repo) : base("customercreated", config.ClientConnection)
//        {
//            this.repo = repo;
//            RegisterForMessages(Handle);
//        }
//
//        private async void Handle(MessagePackage<CustomerDTO> message)
//        {
//            var customer = message.Entity;
//            
//            var cart = await repo.GetCartByCustomerAsync(customer.AnonymousId);
//            cart.UpdateCustomer(customer.AnonymousId, customer.Id);
//            await repo.SaveAsync(cart);
//
//        }
//         
//        
//        public override async Task SendMessage(CustomerDTO doc)
//        {
//            // var topicClient = (ITopicClient)sender; 
//            var message = GetBasicMessage(doc);
//            // message.UserProperties.Add(Configuration.TopicDocumentFilter, doc.SubmissionId.ToString());
//
//            await base.SendMessage(message);
//        }
//    }



}