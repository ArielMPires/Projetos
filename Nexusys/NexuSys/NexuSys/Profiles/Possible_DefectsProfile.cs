using AutoMapper;
using NexuSys.DTOs.Possible_Defects;
using NexuSys.Entities;

public class PossibleDefectsProfile : Profile
{
    public PossibleDefectsProfile()
    {
        CreateMap<Possible_Defects, Possible_DefectsDTO>();

        CreateMap<Possible_Defects, ByPossible_DefectsDTO>()
            .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
            .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
            .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
            .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

        CreateMap<NewPossible_DefectsDTO, Possible_Defects>()
            .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
            {
                CreateBy = src.CreateBy,
                DateCreate = src.DateCreate
            }));

        CreateMap<EditPossible_DefectsDTO, Possible_Defects>()
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
