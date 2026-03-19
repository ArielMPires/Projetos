using AutoMapper;
using NexuSys.DTOs.Purchase;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<Purchase, PurchaseDTO>();

            CreateMap<NewPurchaseDTO, Purchase>()
                                .ForMember(dest => dest.NfsFK, o => o.MapFrom(src => new NFs
                                {
                                    Number = src.NF,
                                    Total_Value = src.Value_total,
                                    DateIn = DateTime.Now,
                                    Tax = 0,
                                    Type = 3,
                                    Customers = 927,
                                    folderFK = new FileFolder
                                    {
                                        Name = src.NF.ToString(),
                                        historyFK = new History
                                        {
                                            CreateBy = src.CreateBy,
                                            DateCreate = src.DateCreate
                                        }
                                    }
                                }));

            CreateMap<EditPurchaseDTO, Purchase>();
        }
    }
}