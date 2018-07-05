using System;
using Shared.Domain;

namespace ShoppingCart.Domain
{
    public class ProductPriceChanged
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public Money OldPrice { get; set; }
        public Money NewPrice { get; set; }
    }
}