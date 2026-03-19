using AutoMapper;
using Domus.DTO.NF;
using Domus.DTO.Passwords;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class PasswordProfile : Profile
    {
        public PasswordProfile()
        {
            CreateMap<Passwords, PasswordDTO>()
                .ForMember(dest => dest.Type, o => o.MapFrom(e => e.TypeFK.Name))
                .ForMember(dest => dest.Owner, o => o.MapFrom(e => e.OwnerFK.Name));
            CreateMap<PasswordDTO, Passwords>();
        }
    }
}
