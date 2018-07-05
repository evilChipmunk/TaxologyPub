using System;
using AutoFixture;
using Domain.Tests.Framework.Customizations;

namespace Domain.Tests.Framework.Customizations
{
    internal class CardCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var rand = new Random();

                var name = fixture.Create<string>().Substring(0, rand.Next(1, 21));
                var sommething = fixture.Create<SomeObject>(); 
                var id =  fixture.Create<Guid>();

                return new Card(id );
            });
        }
    }
}

public class SomeObject
{
    public string SomeName { get; set; }
}
public class Card
{
    public Card(Guid id)
    {
        this.Id   = id;
    }

    public Guid Id { get; set; }
}