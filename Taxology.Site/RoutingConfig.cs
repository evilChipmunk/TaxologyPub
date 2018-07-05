namespace Taxology.Site
{
   
        public class ServiceRoutingConfig
        {
            public string URL { get; set; }
        }

    public class CustomerServiceRoutingConfig : ServiceRoutingConfig
    {
        public string Create { get; set; }
        public string Get { get; set; }
        public string Update { get; set; }
    }

    public class ProductServiceRoutingConfig : ServiceRoutingConfig
    {
        public string GetAllProducts { get; set; }
    }

    public class ShoppingCartRoutingConfig : ServiceRoutingConfig
    {
        public string GetCart { get; set; }
        public string ProductCount { get; set; } 
        public string AddProduct { get; set; }
        public string RemoveProduct { get; set; }

        public string ClearCart { get; set; }
    }

    public class PurchasedProductServiceRoutingConfig : ServiceRoutingConfig
    {
        public string ProductCount { get; set; }
        public string Products { get; set; }
    }

    public class OrderServiceRoutingConfig : ServiceRoutingConfig
    {
        public string GetBillingAddress { get; set; }
        public string CreateOrder { get; set; }

        public string CompleteOrder { get; set; }
        public string CancelOrder { get; set; }
        public string GetOpenOrder { get; set; }

        public string GetOrder { get; set; }
        public string GetCustomerOrders { get; set; }
    }
    public class RoutingConfig
    {

        public string URL { get; set; }

        public CustomerServiceRoutingConfig CustomerService { get; set; }

        public ProductServiceRoutingConfig ProductService { get; set; }

        public ShoppingCartRoutingConfig ShoppingCartService { get; set; }

        public PurchasedProductServiceRoutingConfig PurchasedProductService { get; set; }

        public OrderServiceRoutingConfig OrderService { get; set; }
   
    }
}