using AutoMapper;
using Domus.DTO.NF;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class NFProfile : Profile
    {
        public NFProfile()
        {
            CreateMap<NF_Input, InputDTO>()
                .ForMember(dest => dest.Supplier, o => o.MapFrom(e => e.SupplierFK.Name));
            CreateMap<NF_Output, OutputDTO>()
                .ForMember(dest => dest.Unit, o => o.MapFrom(e => e.UnitFK.Name));
        }
    }
}
