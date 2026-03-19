using AutoMapper;
using NexuSys.DTOs.Type_Service;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class Type_ServiceProfile : Profile
    {
        public Type_ServiceProfile()
        {
            CreateMap<Type_Service, Type_ServiceDTO>();

            CreateMap<Type_Service, ByType_ServiceDTO>()
                 .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                 .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                 .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                 .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewType_ServiceDTO, Type_Service>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByType_ServiceDTO, Type_Service>()
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