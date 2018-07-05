using System.Collections.Generic;

namespace Taxology.Site.Models
{
    public class ProductListModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
    }
}