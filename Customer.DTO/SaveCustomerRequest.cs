namespace Customer.DTO
{
    public class SaveCustomerRequest
    {
        public CustomerDTO Customer { get; set; }
    }

    public interface ISaveCustomerRequest
    {
        CustomerDTO Customer { get; set; }
    }
}