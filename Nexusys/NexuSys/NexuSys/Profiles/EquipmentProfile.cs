using AutoMapper;
using NexuSys.DTOs.Equipment;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class EquipmentProfile : Profile
    {
        public EquipmentProfile()
        {
            CreateMap<Equipment, EquipmentDTO>()
               .ForMember(dest => dest.Customer, o => o.MapFrom(src => src.customersFK.Name))
               .ForMember(dest => dest.Product, o => o.MapFrom(src => src.productsFK.SAP_Description));

            CreateMap<Equipment, ByEquipmentDTO>()
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewEquipmentDTO, Equipment>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<EditEquipmentDTO, Equipment>()
    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
    {
        ID = src.History,
        ChangedBy = src.ChangedBy,
        DateChanged = src.DateChanged
    }));

        }
    }
}
