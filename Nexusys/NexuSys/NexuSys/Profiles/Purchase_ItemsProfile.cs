using AutoMapper;
using NexuSys.DTOs.Purchase_Items;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class Purchase_ItemsProfile : Profile
    {
        public Purchase_ItemsProfile()
        {
            CreateMap<Purchase_Items, Purchase_ItemsDTO>()
              .ForMember(dest => dest.Products, o => o.MapFrom(src => src.productsFK.SAP_Description))
              .ForMember(dest => dest.Purchase, o => o.MapFrom(src => src.purchaseFK.suppliersFK));



            CreateMap<Stock, ByPurchase_ItemsDTO>();


            CreateMap<NewPurchase_ItemsDTO, Purchase_Items>();

            CreateMap<EditPurchase_ItemsDTO, Purchase_Items>();

        }
    }
}
