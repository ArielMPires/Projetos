using AutoMapper;
using Domus.DTO.Request;
using Domus.Models.DB;

namespace Domus.Profiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<Request, RequestDTO>()
                .ForMember(dest => dest.Requester, o => o.MapFrom(e => e.RequesterFK.Name))
                .ForMember(dest => dest.Department, o => o.MapFrom(e => e.DepartmentFK.Name))
                .ForMember(dest => dest.Use, o => o.MapFrom(e => e.UseFK.Name))
                .ForMember(dest => dest.isAuthorization, o => o.MapFrom(e => e.AuthorizationFK.Situation == true));
        }
    }
}
