//using Domain;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Subscribers
{
//    public class CompletedTransormSubscription : BaseSubscriptionClient<ExtractedDocument>
//    {
//        public CompletedTransormSubscription(string subscriptionName) : base(new SubscriptionClient(Configuration.StorageQueue, Configuration.CompletedTransforms, subscriptionName))
//        {
//            this.subscriptionName = subscriptionName; 
//            CreateFilter().GetAwaiter().GetResult();
//        }
//
//        private string subscriptionName;
//         
//        private async Task CreateFilter()
//        {
//            ISubscriptionClient client = (ISubscriptionClient)receiver;
//
//            // 2nd Subscription: Add SqlFilter on Subscription 2
//            // Delete Default Rule.
//            // Add the required SqlFilter Rule
//            // Note: Does not apply to this sample but if there are multiple rules configured for a 
//            // single subscription, then one message is delivered to the subscription when any of the 
//            // rule matches. If more than one rules match and if there is no `SqlRuleAction` set for the
//            // rule, then only one message will be delivered to the subscription. If more than one rules
//            // match and there is a `SqlRuleAction` specified for the rule, then one message per `SqlRuleAction`
//            // is delivered to the subscription. 
//             
//            await client.RemoveRuleAsync(RuleDescription.DefaultRuleName);
//            await client.AddRuleAsync(new RuleDescription
//            {
//
//                //Filter = SubmissionId = whatever the submission guid is
//                Filter = new SqlFilter(string.Format("{0} = '{1}'", Configuration.TopicDocumentFilter, subscriptionName)),
//                Name = "SubmissionIdFilter"
//            });
//            //await client.RemoveRuleAsync(RuleDescription.DefaultRuleName);
//            //await client.AddRuleAsync(new RuleDescription
//            //{
//            //    Filter = new CorrelationFilter() { Label = "SubmissionId", CorrelationId = "subscriptionName" },
//            //    Name = "SubscriptionFilter"
//            //});
//
//
//        }
//    }
}
