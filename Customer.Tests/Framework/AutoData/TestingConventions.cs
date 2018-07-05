using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Tests.Framework.Customizations;

namespace Domain.Tests.Framework.AutoData
{
    internal class TestingConventions : CompositeCustomization
    {
        public TestingConventions()
            : base( 
                new StateCustomization(), 
                new AutoMoqCustomization())
        {
        }
    }
}