using AutoMapper;
using Domus.DTO.Service_Type;
using Domus.DTO.Users;
using Domus.Models.DB;
namespace Domus.Profiles
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<Service_Type, TypeDTO>()
    .ForMember(dest => dest.Category, o => o.MapFrom(src => src.CategoryFK.Name));
        }
    }
}
