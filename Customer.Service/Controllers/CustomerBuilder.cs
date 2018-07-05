using System;
using Customer.DTO;

namespace Customer.Service.Controllers
{
    public interface ICustomerBuilder
    {
        Domain.Customer Build(RegisteredUserDTO dto);
        Domain.Customer Build(CustomerDTO dto);
    }

    public class CustomerBuilder : ICustomerBuilder
    {

        public Domain.Customer Build(RegisteredUserDTO dto)
        {
            return Domain.Customer.Create(dto.FirstName, dto.LastName, dto.Email, dto.AnonymousId);

        }

        public Domain.Customer Build(CustomerDTO dto)
        {
            if (dto.Id == Guid.Empty)
            {
                return Domain.Customer.Create(dto.FirstName, dto.LastName, dto.Email, dto.Phone, dto.StateAbbreviation, dto.AnonymousId);
            }
            else
            {
                return Domain.Customer.CreateExisting(dto.Id, dto.FirstName, dto.LastName, dto.Email, dto.Phone, dto.StateAbbreviation);
            } 
        }
    }
}