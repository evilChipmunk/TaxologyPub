namespace Customer.DTO
{
    public class CreateCustomerRequest
    {
        public RegisteredUserDTO RegisteredUser { get; set; }

    }

    public interface ICreateCustomerRequest
    {
        RegisteredUserDTO RegisteredUser { get; set; }
    }
}