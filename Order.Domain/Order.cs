using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace Order.Domain
{

    //Can be an Entity, or could be created from an Entity if separation is needed from DB models
    //Setters are private, but still able to be mapped by EF
    public class Order : BaseEntity
    {
        private List<Product> products;
        private decimal subTotalAmount;
        private decimal totalAmount;

        private Order()
        {
            products = new List<Product>();
        }


        //Immutable class pattern - Order checks its invariant data at construction, state changes must
        //happen through methods not just property setters to ensure that the Order object is always
        //in a valid state. Factory methods used because they have more flexibility than constructors
        //with exception raising, virtual calls, etc
        public static Order CreateNew(Customer customer, string address1, string address2,
            string country, string phone, string stateAbbreviation, string zipCode, decimal taxRate)
        {
            Guard.ForNull(customer, "customer");
            Guard.ForNullOrEmpty(address1, "address1");
            Guard.ForNullOrEmpty(country, "country");
            Guard.ForNullOrEmpty(phone, "phone");
            Guard.ForNullOrEmpty(stateAbbreviation, "stateAbbreviation");
            Guard.ForNullOrEmpty(zipCode, "zipCode");

            Guard.ForNullOrEmpty(stateAbbreviation, "stateAbbreviation");
            var foundState = States.GetByAbbreviation(stateAbbreviation);
            Guard.ForNull(foundState, "stateAbbreviation", "Invalid state abbreviation");

            Order order = CreateOrder(customer, taxRate);
            order.Id = Guid.NewGuid();
            order.State = SaveState.UnSaved;
            order.OrderStatus = OrderStatus.Created;


            //Double Dispatch pattern to create BillingAddress - Billing only has reference to an order ID
            //imposing a traversal direction that is not bi-directional (not needed for this object)
            //BillingAddress has its own Guard/invariant checks
            order.BillingAddress = BillingAddress.Create(order, address1, address2, country, phone,
                stateAbbreviation, zipCode);

            //domain events added for any event sourcing needs - are published to consumers after save transaction completes
            order.AddEvent(new OrderCreatedEvent(order));
            return order;
        }


        public static Order CreateExisting(Guid id, Customer customer, BillingAddress billing,
            decimal taxRate, OrderStatus status)
        {
            Guard.ForNullOrEmpty(id, "id");
            Guard.ForNull(billing, "billing");

            Order order = CreateOrder(customer, taxRate);
            order.Id = id;
            order.State = SaveState.Saved;
            order.OrderStatus = status;
            order.BillingAddress = billing;

            order.AddEvent(new OrderHydratedEvent(order));

            return order;
        }

        private static Order CreateOrder(Customer customer, decimal taxRate)
        {
            Guard.ForNull(customer, "customer");
            Guard.ForLessEqualZero(taxRate, "taxRate");

            Order order = new Order();

            order.CustomerId = customer.Id; 
            order.OrderDate = DateTime.Now;

            order.TaxRate = taxRate;
            order.Tax = new Money(0).Amount;
            order.Exchange = new Money(taxRate).Exchange;

            return order;
        }


        //Just basic getter properties
        public DateTime OrderDate { get; private set; }
        public BillingAddress BillingAddress { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Exchange { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        protected decimal TaxRate { get; private set; }


        //Properties that interact with the current state of the domain object
        public IEnumerable<Product> Products
        {
            get { return products.AsEnumerable();}
            private set { products = new List<Product>(value); }
        }

        public decimal SubTotal
        {
            get
            {
                subTotalAmount = 0;

                foreach (var product in Products)
                {
                      subTotalAmount += product.Price;
                }

                return subTotalAmount;
            }
            private set { subTotalAmount = value; }
        }


        public decimal Tax
        {
            get
            { 
                return SubTotal *  TaxRate;
            }
            private set { }
        }
        

        public decimal Total
        {
            get
            {
                return  SubTotal + Tax; 
            }
            private set { totalAmount = value; }
        }

        

        //Behavioral methods
        public void Confirm()
        {
            this.OrderStatus = OrderStatus.Confirmed;  
            this.AddEvent(new OrderConfirmedEvent(this));
        }

        public void Cancel()
        {
            this.OrderStatus = OrderStatus.Cancelled;
            this.AddEvent(new OrderCanceledEvent(this));
        }

        public void AddProduct(Guid productId, string name, decimal price)
        {
            //either add a new product, or updated the quantity of an existing one
            var foundProduct = products.FirstOrDefault(x => x.Id == productId);
            if (foundProduct != null)
            {
                foundProduct.Quantity++;
            }
            else
            {
                this.products.Add(Product.Create(productId, name, price, this));
            }
        }


        //Needs a domain service. Domain services contain logic that might be shared by domain models,
        //or has some type of operation that shouldn't be the responsibility of the domain object
        //Domain services are not the same as Infrastructure services (doing plumbing work, repository/database access)
        public void ChangeTaxRate(ITaxService taxDomainService)
        {
            this.TaxRate = taxDomainService.GetTaxRate(this.BillingAddress.StateAbbreviation);
        }
    }
}