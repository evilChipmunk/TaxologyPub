using AutoMapper;
using Customer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Service.Mappings
{
    public class CustomerMappings : Profile
    {
        public CustomerMappings()
        {
            CreateMap<CustomerDTO, Domain.Customer>();


            CreateMap<CustomerDTO, GetCustomerResponse>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src));

            CreateMap<CustomerDTO, SaveCustomerResponse>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src));


            CreateMap<IEnumerable<CustomerDTO>, GetAllCustomersResponse>()
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src));
        }
    }
}
