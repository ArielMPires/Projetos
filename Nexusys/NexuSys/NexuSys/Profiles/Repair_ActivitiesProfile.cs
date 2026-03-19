using AutoMapper;
using NexuSys.DTOs.Repair_Activities;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class Repair_ActivitiesProfile : Profile
    {
        public Repair_ActivitiesProfile()
        {
            CreateMap<Repair_Activities, Repair_ActivitiesDTO>();

            CreateMap<Repair_Activities, ByRepair_ActivitiesDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewRepair_ActivitiesDTO, Repair_Activities>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<EditRepair_ActivitiesDTO, Repair_Activities>()
    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
    {
        ID = src.History,
        ChangedBy = src.ChangedBy,
        DateChanged = src.DateChanged
    }));

        }
    }
}
