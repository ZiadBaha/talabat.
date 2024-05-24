using AutoMapper;
using talabat.Apis.Dtos;
using talabat.core.Entites;
using talabat.core.Entites.identity;
using talabat.core.Entites.Order_Aggregate;

namespace talabat.Apis.Helpers
{
    public class MappingProfilles : Profile
    {
        public MappingProfilles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductBrand , o=>o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureURL, o => o.MapFrom<productpictureurlresolver>());


            CreateMap<talabat.core.Entites.identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketitemDto, Basketitem>();
            CreateMap<OrderAddressDto, talabat.core.Entites.Order_Aggregate.Address>();


        }
    }
}
