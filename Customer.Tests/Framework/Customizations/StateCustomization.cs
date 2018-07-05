using System;
using AutoFixture;
using Shared.Domain;

namespace Domain.Tests.Framework.Customizations
{
    internal class StateCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var states = States.ToList();

                var rand = new Random();

                var stateIndex = rand.Next(0, states.Count - 1);

                return states[stateIndex];
            });
        }
    }
}