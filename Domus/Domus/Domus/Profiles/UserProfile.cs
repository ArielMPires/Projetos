using AutoMapper;
using Domus.DTO.Users;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDTO>()
                .ForMember(dest => dest.Role, o => o.MapFrom(src => src.RoleFK.Name))
                .ForMember(dest => dest.Department, o => o.MapFrom(src => src.DepartmentFK.Name));
            CreateMap<ThemeDTO, Theme>();
            CreateMap<Theme, ThemeIdDTO>();

            CreateMap<CreateUserDTO, Users>();
            CreateMap<NewPhotoDTO, Users>();

        }
    }
}
