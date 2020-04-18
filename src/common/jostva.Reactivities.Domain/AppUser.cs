using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace jostva.Reactivities.Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public virtual ICollection<UserActivity> UserActivities { get; set; }
    }
}