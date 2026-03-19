using System.Security;
using AutoMapper;
using NexuSys.DTOs.Permissions;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class PermissionsProfile : Profile
    {
        public PermissionsProfile()
        {
            CreateMap<Permissions, PermissionsDTO>()
             .ForMember(dest => dest.Role, o => o.MapFrom(src => src.rolefk.Name));




            CreateMap<Permissions, ByPermissionsDTO>();



            CreateMap<NewPermissionsDTO, Permissions>();

            CreateMap<EditPermissionsDTO, Service_Order>()
    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
    {
      
        ChangedBy = src.ChangedBy,
        DateChanged = src.DateChanged
    }));

        }
    }
}
