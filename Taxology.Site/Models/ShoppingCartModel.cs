using System;
using System.Collections.Generic; 

namespace Taxology.Site.Models
{
    public class ShoppingCartModel
    { 
        public Guid Id { get; set; }

        public List<ProductModel> Products { get; set; }

        public double Total { get; set; }

    }
}
