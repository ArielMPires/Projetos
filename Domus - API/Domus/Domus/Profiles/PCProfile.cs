using AutoMapper;
using Domus.DTO.Computer;
using Domus.DTO.Patrimony;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class PCProfile : Profile
    {
        public PCProfile()
        {
            CreateMap<Computer, PCDTO>()
.ForMember(dest => dest.Description, o => o.MapFrom(src => src.PatrimonyFK.Description));
        }
    }
}
