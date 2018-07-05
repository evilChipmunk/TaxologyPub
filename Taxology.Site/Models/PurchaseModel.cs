using System;

namespace Taxology.Site.Models
{
    [Serializable]
    public class PurchaseModel
    {
        public PurchaseModel()
        {
            Billing = new BillingInfoModel();
            Order = new OrderModel();
        }
        public Guid CustomerId { get; set; }

        public BillingInfoModel Billing { get; set; }

        public Guid CartId { get; set; } 

        public OrderModel Order { get; set; }

        public int Step { get; set; }
         
    }
}