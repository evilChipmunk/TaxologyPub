using System;
using Shared.DTO;

namespace Product.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ImageLink { get; set; }

        public MoneyDTO Price { get; set; }

        public string Description { get; set; }
    }

    public interface IProductChanged
    {
        ProductDTO Product { get; set; }
    }
}