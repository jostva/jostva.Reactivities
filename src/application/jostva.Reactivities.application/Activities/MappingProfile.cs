using AutoMapper;
using jostva.Reactivities.Domain;
using System.Linq;

namespace jostva.Reactivities.application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                            .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(item => item.IsMain).Url));
        }
    }
}