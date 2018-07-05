using System;

namespace Customer.DTO
{
    public class GetCustomerRequest
    {
        public GetCustomerRequest(Guid id)
        {
            this.Id = id;
        }
        public Guid Id { get; set; }
    }

    public interface IGetCustomerRequest{

        Guid Id { get; set; }
    }
}