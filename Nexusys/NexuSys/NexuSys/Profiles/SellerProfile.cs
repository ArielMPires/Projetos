using AutoMapper;
using NexuSys.DTOs.Seller;
using NexuSys.DTOs.Type_Service;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class SellerProfile : Profile
    {
        public SellerProfile()


        {
            CreateMap<Seller, SellerDTO>();

            CreateMap<Seller, BySellerDTO>()
                 .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                 .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                 .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                 .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewSellerDTO, Seller>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<BySellerDTO, Seller>()
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    ID = src.History,
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate,
                    ChangedBy = src.ChangedBy,
                    DateChanged = src.DateChanged
                }));

        }
    }
}