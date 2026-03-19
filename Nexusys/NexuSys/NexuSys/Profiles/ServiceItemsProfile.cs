using AutoMapper;
using NexuSys.DTOs.ServiceItems;
using NexuSys.DTOs.Users;
using NexuSys.Entities;
using NexuSys.Interfaces;

namespace NexuSys.Profiles
{
    public interface ServiceItemsProfile
    {
        public class ServiceItemsProfile : Profile
        {
            public ServiceItemsProfile()
            {
                CreateMap<Service_Items, ServiceItemsDTO>()
                    .ForMember(dest => dest.Product, o => o.MapFrom(src => src.productsFK.SAP_Description));

                CreateMap<Service_Items, ByServiceItemsDTO>()
                    .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                    .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                    .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                    .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

                CreateMap<NewServiceItemsDTO, Service_Items>()
                    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                    {
                        CreateBy = src.CreateBy,
                        DateCreate = src.DateCreate
                    }));

                CreateMap<ByServiceItemsDTO, Service_Items>()
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
}

