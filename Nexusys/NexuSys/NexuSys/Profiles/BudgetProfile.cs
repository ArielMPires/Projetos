using AutoMapper;
using NexuSys.DTOs.Budget;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<Budget, BudgetDTO>();

            CreateMap<Budget, ByBudgetDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewBudgetDTO, Budget>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByBudgetDTO, Budget>()
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
