using AutoMapper;
using Domus.DTO.Service_Order;
using Domus.DTO.Service_Type;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class ServiceOrderProfile : Profile
    {
        public ServiceOrderProfile()
        {
            CreateMap<Service_Order, ServiceOrderDTO>()
                .ForMember(dest => dest.Request, o => o.MapFrom(src => src.RequestFK.Name))
                .ForMember(dest => dest.Technical, o => o.MapFrom(src => src.TechnicalFK.Name))
                .ForMember(dest => dest.Type, o => o.MapFrom(src => src.TypeFK.Name))
                .ForMember(dest => dest.Priority, o => o.MapFrom(src => src.TypeFK.Priority))
                .ForMember(dest => dest.Category, o => o.MapFrom(src => src.TypeFK.CategoryFK.Name))
                .ForMember(dest => dest.Conclude_Date, o => o.MapFrom(src => src.ConcludeFK.Date));

            CreateMap<CatchOrderDTO,Service_Order>()
                .ForMember(dest => dest.Technical, opt => opt.MapFrom(src => src.Technical))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.DateChanged, opt => opt.MapFrom(src => src.DateChanged))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<EndOrderDTO, Service_Order>()
                .ForMember(dest => dest.ConcludeFK,
                opt => opt.MapFrom(src => new End_Call { Date = src.DateChanged, Technical = src.ChangedBy, Reason = src.Reason }));
        }
    }
}
