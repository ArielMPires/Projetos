using AutoMapper;
using NexuSys.DTOs.Nfs;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class NFsProfile : Profile
    {
        public NFsProfile()
        {
            CreateMap<NFs, NFsDTO>()
                .ForMember(dest => dest.Customers, o => o.MapFrom(src => src.customersFK.Name));

            CreateMap<NFs, ByNFsDTO>();

            CreateMap<NewNFsDTO, NFs>();

            CreateMap<EditNFsDTO, NFs>();

        }
    }
}
