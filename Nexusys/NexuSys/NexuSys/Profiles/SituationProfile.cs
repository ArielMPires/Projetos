using AutoMapper;
using NexuSys.DTOs.SItuation;
using NexuSys.DTOs.Unit;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class SituationProfile : Profile
    {
        public SituationProfile()
        {
            CreateMap<Situation, SituationDTO>();
            CreateMap<Situation, BySituationDTO>()
                 .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                 .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                 .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                 .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewSituationDTO, Situation>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<BySituationDTO, Situation>()
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