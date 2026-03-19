using AutoMapper;
using NexuSys.DTOs.Suppliers;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class SuppliersProfile : Profile
    {
        public SuppliersProfile()
        {
        
            CreateMap<Suppliers, SuppliersDTO>();
            CreateMap<Suppliers, BySuppliersDTO>();

         
            CreateMap<NewSuppliersDTO, Suppliers>();
            CreateMap<BySuppliersDTO, Suppliers>();
        }
    }
}