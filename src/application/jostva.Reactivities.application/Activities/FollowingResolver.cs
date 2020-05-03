using AutoMapper;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace jostva.Reactivities.application.Activities
{
    public class FollowingResolver : IValueResolver<UserActivity, AttendeeDto, bool>
    {
        private readonly DataContext dataContext;
        private readonly IUserAccessor userAccessor;


        public FollowingResolver(DataContext dataContext, IUserAccessor userAccessor)
        {
            this.dataContext = dataContext;
            this.userAccessor = userAccessor;
        }


        public bool Resolve(UserActivity source, AttendeeDto destination, bool destMember, ResolutionContext context)
        {
            AppUser currentUSer = dataContext.Users.SingleOrDefaultAsync(x => x.UserName == userAccessor.GetCurrentUsername()).Result;

            if (currentUSer.Followings.Any(x => x.TargetId == source.AppUserId))
            {
                return true;
            }

            return false;
        }
    }
}