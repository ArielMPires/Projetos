using AutoMapper;
using NexuSys.DTOs.ServiceEquipment;
using NexuSys.DTOs.Users;
using NexuSys.Entities;
using NexuSys.Interfaces;

namespace NexuSys.Profiles
{
    public interface ServiceEquipmentProfile
    {
        public class ServiceEquipmentProfile : Profile
        {
            public ServiceEquipmentProfile()
            {
                CreateMap<Service_Equipment, ServiceEquipmentDTO>()
                    .ForMember(dest => dest.Equipment, o => o.MapFrom(src => src.equipmentFK.productsFK.SAP_Description +" #" + src.equipmentFK.Serial ));


                CreateMap<Service_Equipment, ByServiceEquipmentDTO>()
                    .ForMember(dest => dest.EquipmentCompleto, o => o.MapFrom(src => src.equipmentFK.productsFK.SAP_Description + " #" + src.equipmentFK.Serial))
                    .ForMember(dest => dest.Equipment, o => o.MapFrom(src => src.equipmentFK.productsFK.SAP_Description))
                    .ForMember(dest => dest.Product, o => o.MapFrom(src => src.equipmentFK.Product))
                    .ForMember(dest => dest.Serial, o => o.MapFrom(src => src.equipmentFK.Serial))
                    .ForMember(dest => dest.Manufacturing_Date, o => o.MapFrom(src => src.equipmentFK.Manufacturing_Date))
                    .ForMember(dest => dest.Optional, o => o.MapFrom(src => src.equipmentFK.Optional));

                CreateMap<NewServiceEquipmentDTO, Service_Equipment>();

                CreateMap<EditServiceEquipmentDTO, Service_Equipment>();

            }
        }
    }
}

