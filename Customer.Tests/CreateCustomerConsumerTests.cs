using System;
using AutoMapper;
using Customer.DTO;
using Customer.Persistency;
using Customer.Service;
using Domain.Tests.Framework.AutoData;
using MassTransit;
using Moq;
using Xunit;

namespace Customer.Tests
{
    public class CreateCustomerConsumerTests
    { 
        [Theory, AutoMoqData]
        public void Should_Return_Response( IMapper mapper,
        Mock<ICustomerBuilder> builder, 
        Mock<ICustomerRepository> repo,    
        Mock<ConsumeContext<ICreateCustomerRequest>> context, 
        Mock<ICreateCustomerRequest> request, RegisteredUserDTO dto)
        {
            //could create a customization for this (see Framework/Customizations folder in this project for example)
            Domain.Customer customer = Domain.Customer.Create(dto.FirstName, dto.LastName, dto.Email, dto.AnonymousId);


            //just stuff to make the test move forward
            request.Setup(x => x.RegisteredUser).Returns(dto);
            builder.Setup(x => x.Build(dto)).Returns(customer);

            repo.Setup(x => x.SaveAsync(customer)).ReturnsAsync(customer);

            context.Setup(x => x.Message).Returns(request.Object);


            //call back for the context.RespondAsync -- point of this test is to make sure
            //that a response is created, normally this would go to the message queue.
            //The Consume method doesn't give a direct response. This could also be handled 
            //by creating an in-memory queue for masstransit
            CreateCustomerResponse returnedResponse = null;
            Action<CreateCustomerResponse> testCallBack = (x) =>
            {
                returnedResponse = x;
            };

            
            context.Setup(x => x.RespondAsync(It.IsAny<CreateCustomerResponse>()))
                .Callback(testCallBack); 

            var sut = new CreateCustomerConsumer(builder.Object, repo.Object, mapper);

            sut.Consume(context.Object).ConfigureAwait(false); 
            

            Assert.NotNull(returnedResponse);
        } 
    }
}