using jostva.Reactivities.application.Errors;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly DataContext dataContext;
        private readonly IUserAccessor userAccessor;


        public ProfileReader(DataContext dataContext, IUserAccessor userAccessor)
        {
            this.dataContext = dataContext;
            this.userAccessor = userAccessor;
        }


        public async Task<Profile> ReadProfile(string username)
        {
            AppUser user = await dataContext.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
            }

            AppUser currentUser = await dataContext.Users.SingleOrDefaultAsync(x => x.UserName == userAccessor.GetCurrentUsername());

            Profile profile = new Profile
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = user.Photos,
                Bio = user.Bio,
                FollowersCount = user.Followers.Count(),
                FollowingCount = user.Followings.Count()
            };

            if (currentUser.Followings.Any(x => x.TargetId == user.Id))
            {
                profile.IsFollowed = true;
            }

            return profile;
        }
    }
}