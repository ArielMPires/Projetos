using AutoMapper;
using Domus.DTO.Products;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Products, ProductsDTO>()
                .ForMember(dest => dest.Category, o => o.MapFrom(e => e.CategoryFK.Name))
                .ForMember(dest => dest.Mark, o => o.MapFrom(e => e.MarkFK.Name))
                .ForMember(dest => dest.Amout, o => o.MapFrom(e => e.ProductsFK.Amount))
                .ForMember(dest => dest.Measure, o => o.MapFrom(e => e.ProductsFK.Measure));
        }
    }
}
