using System;
using Xunit.Sdk;

namespace Domain.Tests.Framework.Categories
{
    // Pay attention on [TraitDiscoverer("", "")]
    // the first argument is “The fully qualified type name of the discoverer
    // (f.e., ‘Xunit.Sdk.TraitDiscoverer’)” and the second argument is
    // “The name of the assembly that the discoverer type is located in, without file extension (f.e., ‘xunit.execution’)“
    // https://dennymichael.net/2015/01/23/how-to-extendmigrate-traitattribute-to-xunit-v2-and-why-has-become-sealed/

    /// <summary>
    ///     Apply this attribute to your test method to specify a category.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    [TraitDiscoverer("Domain.Tests.Framework.Categories.CategoryDiscoverer", "Domain.Tests")]
    internal class CategoryAttribute : Attribute, ITraitAttribute
    {
        internal CategoryAttribute(string category)
        {
        }
    }
}