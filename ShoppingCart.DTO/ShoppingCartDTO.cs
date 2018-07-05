using System;
using System.Collections.Generic;
using Shared.DTO;

namespace ShoppingCart.DTO
{
    public class ShoppingCartDTO
    { 
        public Guid Id { get; set; }

        public List<ShoppingCartProductDTO> Products { get; set; }
 
        public Guid CustomerId { get; set; }

        public MoneyDTO Total { get; set; }
    }
}