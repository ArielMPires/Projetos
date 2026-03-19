using AutoMapper;
using NexuSys.DTOs.Review_Defects;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class Review_DefectsProfile : Profile
    {
        public Review_DefectsProfile()
        {
            CreateMap<Review_Defects, Review_DefectsDTO>()
                .ForMember(dest => dest.Possible_Defects, o => o.MapFrom(src => src.DefectsFK.Name));

            CreateMap<Review_Defects, ByReview_DefectsDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewReview_DefectsDTO, Review_Defects>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByReview_DefectsDTO, Review_Defects>()
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
