using AutoMapper;
using NexuSys.DTOs.Review_Activies;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class Review_ActivitiesProfile : Profile
    {
        public Review_ActivitiesProfile()
        {
            CreateMap<Review_Activities, Review_ActivitiesDTO>()
                .ForMember(dest => dest.Repair_Activities, o => o.MapFrom(src => src.ActivitiesFK.Name));

            CreateMap<Review_Activities, ByReview_ActivitiesDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewReview_ActivitiesDTO, Review_Activities>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByReview_ActivitiesDTO, Review_Activities>()
    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
    {
        ID = src.History,
        ChangedBy = src.ChangedBy,
        DateChanged = src.DateChanged,
        DateCreate = src.DateCreate,
        CreateBy = src.CreateBy
    }));

        }
    }
}
