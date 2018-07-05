using System;
using System.Threading.Tasks;
using MassTransit;
using Shared.Messaging.Commands;

namespace Taxology.WebAPI
{
 


    public class UpdateCustomerConsumer :
        IConsumer<UpdateCustomerAddress>
    {
        public async Task Consume(ConsumeContext<UpdateCustomerAddress> context)
        {
            await Console.Out.WriteLineAsync($"Updating customer: {context.Message.CustomerId}");

            // update the customer address
        }
    }
}