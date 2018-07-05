using System;
using Shared.DTO;

namespace Order.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MoneyDTO Price { get; set; }
    }
}