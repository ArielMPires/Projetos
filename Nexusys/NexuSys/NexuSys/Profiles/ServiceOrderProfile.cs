using AutoMapper;
using NexuSys.DTOs.Budget;
using NexuSys.DTOs.Review;
using NexuSys.DTOs.Review_Activies;
using NexuSys.DTOs.Review_Defects;
using NexuSys.DTOs.ServiceEquipment;
using NexuSys.DTOs.ServiceItems;
using NexuSys.DTOs.ServiceOrder;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class ServiceOrderProfile : Profile
    {
        public ServiceOrderProfile()
        {


            CreateMap<Service_Order, OSDetails>()
                .ForMember(d => d.ItemsFK, o => o.MapFrom(s => s.ItemsFK))
                .ForMember(d => d.BudgetFK, o => o.MapFrom(s => s.BudgetFK))
                .ForMember(d => d.ReviewFK, o => o.MapFrom(s => s.BudgetFK.ReviewFK))
                .ForMember(d => d.ReviewActivitiesFK, o => o.MapFrom(s => s.BudgetFK.ReviewFK.ActiviesFK))
                .ForMember(d => d.reviewDefectsFK, o => o.MapFrom(s => s.BudgetFK.ReviewFK.DefectsFK))
                .ForMember(d => d.EquipamentFK, o => o.MapFrom(s => s.service_equipmentFK))
                .ForMember(d => d.TechnicalName, o => o.MapFrom(s => s.TechnicalFK.Name))
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged)); ;



            CreateMap<Service_Order, ServiceOrderDTO>()
            .ForMember(dest => dest.Customer, o => o.MapFrom(src => src.CustomersFK.Name))
            .ForMember(dest => dest.Department, o => o.MapFrom(src => src.departmentFK.Name))
            .ForMember(dest => dest.Unit, o => o.MapFrom(src => src.unitFK.Name))
            .ForMember(dest => dest.Technical, o => o.MapFrom(src => src.TechnicalFK.Name))
            .ForMember(dest => dest.Situation, o => o.MapFrom(src => src.SituationFK.Name));


            CreateMap<Service_Order, ByServiceOrderDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewServiceOrderDTO, Service_Order>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByServiceOrderDTO, Service_Order>()
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
