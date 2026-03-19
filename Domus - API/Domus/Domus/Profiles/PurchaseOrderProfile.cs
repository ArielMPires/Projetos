using AutoMapper;
using Domus.DTO.Purchase_Order;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class PurchaseOrderProfile : Profile
    {
        public  PurchaseOrderProfile()
        {
            CreateMap<Purchase_Order, Purchase_OrderDTO>()
                .ForMember(dest => dest.Supplier, o => o.MapFrom(e => e.SupplierFK.Name))
                .ForMember(dest => dest.Requester, o => o.MapFrom(e => e.RequestFK.RequesterFK.Name));
        }
    }
}
