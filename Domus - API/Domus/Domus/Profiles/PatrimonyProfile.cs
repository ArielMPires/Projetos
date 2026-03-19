using AutoMapper;
using Domus.DTO.Patrimony;
using Domus.DTO.Service_Order;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class PatrimonyProfile : Profile
    {
        public PatrimonyProfile()
        {
            CreateMap<Patrimony, PatrimonyDTO>()
.ForMember(dest => dest.Category, o => o.MapFrom(src => src.CategoryFK.Name))
.ForMember(dest => dest.Department, o => o.MapFrom(src => src.DepartmentFK.Name))
.ForMember(dest => dest.Owner, o => o.MapFrom(src => src.Current_OwnerFK.Name));
        }
    }
}
