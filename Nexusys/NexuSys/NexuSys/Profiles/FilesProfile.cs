using AutoMapper;
using NexuSys.DTOs.Files;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class FilesProfile : Profile
    {
        public FilesProfile()
        {
            CreateMap<NewFileDTO, Files>()
                    .ForMember(dest => dest.historyFK, o => o.MapFrom(src => new History
                    {
                        CreateBy = src.CreateBy,
                        DateCreate = src.DateCreate
                    }));
        }
    }
}
