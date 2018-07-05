using System.Collections.Generic;
using Order.Domain;
using Order.DTO;

namespace Order.Service.Builders
{
    public class OrderBuilder : IOrderBuilder
    {
        private readonly ITaxService taxService;

        public OrderBuilder(ITaxService taxService)
        {
            this.taxService = taxService;
        }

        public Domain.Order Build(Customer customer, BillingAddressDTO baDTO, IEnumerable<ProductDTO> productDTOs)
        {
            var taxRate = taxService.GetTaxRate(baDTO.StateAbbreviation);

            Domain.Order order = Domain.Order.CreateNew(customer,
                baDTO.Address1, baDTO.Address2, baDTO.Country, baDTO.Phone, baDTO.StateAbbreviation, baDTO.ZipCode,
                taxRate);

            foreach (var product in productDTOs)
            {
                order.AddProduct(product.Id, product.Name, product.Price.Amount);
            }

            return order;
        }
    }
}