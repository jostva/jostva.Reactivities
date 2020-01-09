#region usings

using jostva.Reactivities.application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

#endregion

namespace jostva.Reactivities.Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        public string GetCurrentUsername()
        {
            string username = httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(item => item.Type == ClaimTypes.NameIdentifier)?.Value;

            return username;
        }
    }
}