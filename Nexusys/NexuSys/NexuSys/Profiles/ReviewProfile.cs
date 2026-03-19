using AutoMapper;
using NexuSys.DTOs.Review;
using NexuSys.Entities;

namespace NexuSys.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDTO>()
                .ForMember(dest => dest.Budget, o => o.MapFrom(src => src.budgetFK.Value));

            CreateMap<Review, ByReviewDTO>();

            CreateMap<NewReviewDTO, Review>();

            CreateMap<ByReviewDTO, Review>();

        }
    }
}
