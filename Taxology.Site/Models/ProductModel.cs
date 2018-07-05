using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxology.Site.Models
{
    [Serializable]
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public string ImageLink { get; set; }


        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public Guid CartId { get; set; }

        public Guid OrderId { get; set; }
    }

    public class ProductAddedConfirmationModel
    {
        public ProductAddedConfirmationModel(ProductModel product, IEnumerable<ProductModel> suggestions)
        {
            AddedProduct = product;
            SuggestedProducts = suggestions;
        }
        public ProductModel AddedProduct { get; set; }
        public IEnumerable<ProductModel> SuggestedProducts { get; set; }
    }
 
}