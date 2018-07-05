using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxology.Site.Models
{
    [Serializable]
    public class OrderModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
          
        public BillingInfoModel BillingAddress { get; set; }
         
        public Guid CustomerId { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }

        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [DataType(DataType.Currency)]
        public decimal Tax { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

    }
}