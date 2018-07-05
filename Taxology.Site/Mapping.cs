using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Customer.DTO;
using Order.DTO;
using Shared.Authentication;
using Shared.DTO;
using Shared.Mapping;
using ShoppingCart.DTO;
using Taxology.Site.Controllers;
using Taxology.Site.Models;
using CustomerDTO = Customer.DTO.CustomerDTO;
using ProductDTO = Product.DTO.ProductDTO;

namespace Taxology.Site
{
    public class Mapping : Profile
    {
        public Mapping()
        { 
            mapCustomerServiceDTOs();
            mapShoppingCartDTOs();
            mapProductDTOs();
            mapOrderDTOs();
            mapGeneral();

            //Mapper.AssertConfigurationIsValid();
        }

        private void mapGeneral()
        {
            CreateMap<Customer.DTO.CustomerDTO, CustomerOrdersModel>().IgnoreAllNonExisting();
            CreateMap< IEnumerable<Order.DTO.OrderDTO>, CustomerOrdersModel>().IgnoreAllNonExisting();

            CreateMap<CustomerModel, CustomerOrdersModel>().IgnoreAllNonExisting();
            CreateMap<IEnumerable<OrderModel>, CustomerOrdersModel>().IgnoreAllNonExisting();
           

        }

        private void mapCustomerServiceDTOs()
        {
            CreateMap<RegisterModel, RegisteredUserDTO>();
            CreateMap<CustomerDTO, BillingInfoModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.StateAbbreviation, opt => opt.MapFrom(src => src.StateAbbreviation));

           // CreateMap<CustomerModel, CustomerDTO>().IgnoreAllNonExisting();
            CreateMap<CustomerDTO, CustomerModel>().IgnoreAllNonExisting();

            CreateMap<SiteUser, CustomerDTO>().IgnoreAllNonExisting()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));
            CreateMap<BillingInfoModel, CustomerDTO>().IgnoreAllNonExisting();
        }

        private void mapShoppingCartDTOs()
        {
            CreateMap<ShoppingCartProductDTO, ProductModel>()
                .ForMember(src => src.Price, opt => opt.MapFrom(dest => dest.Price.Amount))
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.ProductId));
            CreateMap<ShoppingCartDTO, ShoppingCartModel>()
                .ForMember(src => src.Total, opt => opt.MapFrom(dest => dest.Total.Amount));

            CreateMap<ProductModel, ShoppingCartProductDTO>()
                .ForMember(src => src.ProductId, opt => opt.MapFrom(dest => dest.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new MoneyDTO {Amount = src.Price, Exchange = DefaultExchange.USD}));


        }

        private void mapProductDTOs()
        {
            CreateMap<ProductModel, Product.DTO.ProductDTO>()
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => new MoneyDTO {Amount = src.Price, Exchange = DefaultExchange.USD}));

            CreateMap<ProductDTO, ProductModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

        }

        private void mapOrderDTOs()
        {
            CreateMap<BillingAddressDTO, BillingInfoModel>().IgnoreAllNonExisting();
            CreateMap<OrderDTO, OrderModel>().IgnoreAllNonExisting()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total.Amount))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal.Amount))
                .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax.Amount)).IgnoreAllNonExisting();


            CreateMap<Order.DTO.ProductDTO, ProductModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount)).IgnoreAllNonExisting();

            CreateMap<ProductModel, Order.DTO.ProductDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new MoneyDTO {Amount = src.Price, Exchange = DefaultExchange.USD}));

        }
    }


    public class People
    {
        public string Name { get; set; }
    }

    public class Phone
    {
        public int Number { get; set; }
    }

    public class PeoplePhoneDto
    {
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
    }

    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<People, PeoplePhoneDto>();
            CreateMap<Phone, PeoplePhoneDto>()
                .ForMember(d => d.PhoneNumber, a => a.MapFrom(s => s.Number));

        }

        public PeoplePhoneDto MapIt(People p, Phone phone)
        {
            return Mapper.Map<PeoplePhoneDto>(p).Map(phone);
        }

    }
}
