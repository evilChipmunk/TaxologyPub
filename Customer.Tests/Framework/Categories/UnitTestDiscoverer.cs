using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Domain.Tests.Framework.Categories
{
    internal class UnitTestDiscoverer : ITraitDiscoverer
    {
        /// <summary>
        ///     Gets the trait values from the EndToEnd Category attribute.
        /// </summary>
        /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
        /// <returns>The trait values.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", "Unit");
        }
    }
}