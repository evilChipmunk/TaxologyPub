using System;
using Xunit.Sdk;

namespace Domain.Tests.Framework.Categories
{
    /// <summary>
    ///     Designates the category of test being performed as a Unit Test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    [TraitDiscoverer("Domain.Tests.Framework.Categories.IntegrationTestDiscoverer", "Domain.Tests")]
    internal class IntegrationTestAttribute : Attribute, ITraitAttribute
    {
    }
}