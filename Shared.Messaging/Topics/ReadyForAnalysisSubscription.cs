//using Domain;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Subscribers
{
//    public class ReadyForAnalysisSubscription : BaseSubscriptionClient<Submission>
//    {
//        public ReadyForAnalysisSubscription(string subscriptionName) : base(new SubscriptionClient(Configuration.StorageQueue, Configuration.ReadyForAnalysis, subscriptionName))
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
//            var rules = await client.GetRulesAsync();
//
//            foreach(var rule in rules)
//            {
//                await client.RemoveRuleAsync(rule.Name);
//            }
//            string filterText =string.Format("{0} = '{1}'", Configuration.TopicReadyForAnalysisFilter, subscriptionName);
//            // await client.RemoveRuleAsync(RuleDescription.DefaultRuleName);
//            await client.AddRuleAsync(new RuleDescription
//            {
//                Filter = new SqlFilter(filterText),
//                Name = "StateFilter"
//            }); 
//
//        }
//    }
}
