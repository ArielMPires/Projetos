using AutoMapper;
using NexuSys.DTOs.Products;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Products, ProductsDTO>()
                .ForMember(dest => dest.Stock, o => o.MapFrom(src => src.StockFK.Amount));


            CreateMap<Products, ByProductsDTO>()
                .ForMember(dest => dest.Product, o => o.MapFrom(src => src.StockFK.ID))
                .ForMember(dest => dest.CreateBy, o => o.MapFrom(src => src.historyFK.CreateBy))
                .ForMember(dest => dest.DateCreate, o => o.MapFrom(src => src.historyFK.DateCreate))
                .ForMember(dest => dest.ChangedBy, o => o.MapFrom(src => src.historyFK.ChangedBy))
                .ForMember(dest => dest.DateChanged, o => o.MapFrom(src => src.historyFK.DateChanged));

            CreateMap<NewProductsDTO, Products>()
                .ForMember(dest => dest.StockFK, o => o.MapFrom(src => new Stock
                {
                    Product = src.ID,
                    Amount = 0
                }))
                .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                {
                    CreateBy = src.CreateBy,
                    DateCreate = src.DateCreate
                }));

            CreateMap<ByProductsDTO, Products>()
                .ForMember(dest => dest.StockFK, o => o.MapFrom(src => new Stock
                {
                    ID = src.Product,
                    Product = src.ID,
                    Amount = 0
                }))
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
