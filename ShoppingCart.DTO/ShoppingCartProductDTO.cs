using System;
using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class ShoppingCartProductDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public MoneyDTO Price { get; set; }

    }
}