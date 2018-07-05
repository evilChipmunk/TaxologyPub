using System;
using System.Linq;
using Customer.Domain;
using Domain.Tests.Framework.AutoData;
using Shared.Domain;
using Xunit;

namespace Customer.Tests
{
    public class CustomerTests
    {
        public class GettersAndSetters
        {
            [Theory, AutoMoqData]
            public void CanGet(Guid id, string firstname, string lastName, string email, string phone,
                State state)
            {
                var sut = Customer.Domain.Customer.CreateExisting(id, firstname, lastName,
                    email, phone, state.Abbreviation);

                //this is mainly for test coverate
                ModelsGetSetTest.GettersGetWithoutError(sut);

            }

            [Theory, AutoMoqData]
            public void CanSet(Guid id, string firstname, string lastName, string email, string phone,
                State state)
            {
                var sut = Customer.Domain.Customer.CreateExisting(id, firstname, lastName,
                    email, phone, state.Abbreviation);

                //this is mainly for test coverate
                ModelsGetSetTest.SettersSetWithoutError(sut);

            }
        }

        public class CustomerCreate
        {
            [Theory, AutoMoqData]
            public void Create_Produces_Customer(string firstname, string lastName, string email, 
                Guid anonymousId)
            { 
                var sut = Customer.Domain.Customer.Create(firstname, lastName, email, anonymousId);

                Assert.NotNull(sut); 
            }

            [Theory, AutoMoqData]
            public void Create_Produces_CreatedEvent(string firstname, string lastName, string email,
                Guid anonymousId)
            {
                var sut = Customer.Domain.Customer.Create(firstname, lastName, email, anonymousId);

                Assert.NotEmpty(sut.Events);
                Assert.NotNull(sut.Events.FirstOrDefault(x => x is CustomerCreatedEvent)); 
            }

            [Theory, AutoMoqData]
            public void Create_With_MissingData_Throws(string firstname, string lastName, string email,
                Guid anonymousId)
            {

                Exception firstNameMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.Create(null, lastName, email, anonymousId);
                });
                Exception lastNameMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.Create(firstname, null, email, anonymousId);
                });
                Exception emailMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.Create(firstname, lastName, null, anonymousId);
                }); 
                Exception anonIdMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.Create(firstname, lastName, email, Guid.Empty);
                });

            }
        }

        public class CustomerCreateExisting
        {
            [Theory, AutoMoqData]
            public void Create_Produces_Customer(Guid id, string firstname, string lastName, string email, string phone,
                State state)
            {
                var sut = Customer.Domain.Customer.CreateExisting(id, firstname, lastName,
                    email, phone, state.Abbreviation);

                Assert.NotNull(sut);
            }

            [Theory, AutoMoqData]
            public void Create_Produces_CreatedEvent(Guid id, string firstname, string lastName, string email, string phone,
                State state)
            {
                var sut = Customer.Domain.Customer.CreateExisting(id, firstname, lastName,
                    email, phone, state.Abbreviation);

                Assert.NotEmpty(sut.Events);
                Assert.NotNull(sut.Events.FirstOrDefault(x => x is CustomerHydratedEvent));
            }

            [Theory, AutoMoqData]
            public void Create_With_MissingData_Throws(Guid id, string firstname, string lastName, string email, string phone,
                State state)
            {
                Exception idMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(Guid.Empty, firstname, lastName,
                        email, phone, state.Abbreviation);
                });


                Exception firstNameMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(id, null, lastName,
                        email, phone, state.Abbreviation);
                });
                Exception lastNameMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(id, firstname, null,
                        email, phone, state.Abbreviation);
                });
                Exception emailMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(id, firstname, lastName,
                        null, phone, state.Abbreviation);
                });
                Exception stateMissing = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(id, firstname, lastName,
                        email, phone, null);
                });
            }

            [Theory, AutoMoqData]
            public void Create_With_BadData_Throws(Guid id, string firstname, string lastName, string email, string phone,
                string badData)
            {
                Exception badState = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    Domain.Customer.CreateExisting(id, firstname, lastName,
                        email, phone, badData);
                });
            }
        }
    }
}