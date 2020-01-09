using Microsoft.AspNetCore.Identity;

namespace jostva.Reactivities.Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

    }
}