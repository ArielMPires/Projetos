using AutoMapper;
using NexuSys.DTOs.Stock;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class StokProfile : Profile
    {
        public StokProfile()
        {
            CreateMap<Stock, StockDTO>()
              .ForMember(dest => dest.Product, o => o.MapFrom(src => src.productsFK.SAP_Description));



            CreateMap<Stock, ByStockDTO>();


            CreateMap<NewStockDTO, Stock>();

            CreateMap<EditStockDTO, Service_Order>();

        }
    }
}
