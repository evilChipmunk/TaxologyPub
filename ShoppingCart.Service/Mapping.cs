using AutoMapper;
using Shared.Domain;
using Shared.DTO;
using ShoppingCart.DTO;

namespace ShoppingCart.Service
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.ShoppingCart, ShoppingCartDTO>();
            CreateMap<Domain.ShoppingCartProduct, ShoppingCartProductDTO>();

            CreateMap<Money, MoneyDTO>().ReverseMap();
            CreateMap<ShoppingCartProductDTO, Domain.ShoppingCartProduct>();
        }
    }
}