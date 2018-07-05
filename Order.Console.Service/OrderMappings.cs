using AutoMapper;
using Order.Domain;
using Order.DTO; 
using Shared.DTO;
 

namespace Order.Service
{
    public class OrderMappings : Profile
    {
        public OrderMappings()
        {
            //CreateMap<MoneyDTO, Money>().IgnoreAllNonExisting();
            //CreateMap<BillingAddressDTO, BillingAddress>().IgnoreAllNonExisting();
            //CreateMap<ProductDTO, Product>().IgnoreAllNonExisting();

            CreateMap<Domain.Order, OrderDTO>()
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => new MoneyDTO(src.SubTotal, src.Exchange)))
                .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => new MoneyDTO(src.Tax, src.Exchange)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => new MoneyDTO(src.Total, src.Exchange)));


            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new MoneyDTO(src.Price, src.Exchange)));



            

            //  Mapper.AssertConfigurationIsValid();
        }
    }
}