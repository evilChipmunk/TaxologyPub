//using Domain;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Subscribers
{
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
