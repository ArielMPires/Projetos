using AutoMapper;
using NexuSys.DTOs.Users;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDTO>()
    .ForMember(dest => dest.Department, o => o.MapFrom(src => src.DepartmentFK.Name))
    .ForMember(dest => dest.Role, o => o.MapFrom(src => src.RoleFK.Name))
    .ForMember(dest => dest.MostrarSenha, o => o.Ignore());


            CreateMap<Users, ByUserDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewUserDTO, Users>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<EditUserDTO, Users>()
    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
    {
        ID = src.History,
        CreateBy = src.CreateBy,
        DateCreate = src.DateCreate,
        ChangedBy = src.ChangedBy,
        DateChanged = src.DateChanged
    }));

        }
    }
}
