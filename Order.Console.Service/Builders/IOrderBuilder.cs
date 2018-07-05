using System.Collections.Generic;
using Order.Domain;
using Order.DTO;

namespace Order.Service.Builders
{
    public interface IOrderBuilder
    {
        Domain.Order Build(Customer customer, BillingAddressDTO baDTO, IEnumerable<ProductDTO> productDTOs);
    }
}