using jostva.Reactivities.application.Errors;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.User
{
    public class ExternalLogin    
    {
        public class Query : IRequest<User>
        {
            public string AccessToken { get; set; }
        }


        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IFacebookAccesor facebookAccesor;
            private readonly IJwtGenerator jwtGenerator;


            public Handler(UserManager<AppUser> userManager, IFacebookAccesor facebookAccesor, IJwtGenerator jwtGenerator)
            {
                this.userManager = userManager;
                this.facebookAccesor = facebookAccesor;
                this.jwtGenerator = jwtGenerator;
            }


            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                FacebookUserInfo userInfo = await facebookAccesor.FacebookLogin(request.AccessToken);

                if (userInfo == null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem validating token" });
                }

                AppUser user = await userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        DisplayName = userInfo.Name,
                        Id = userInfo.Id,
                        Email = userInfo.Email,
                        UserName = "fb_" + userInfo.Id
                    };

                    Photo photo = new Photo
                    {
                        Id = "fb_" + userInfo.Id,
                        Url = userInfo.Picture.Data.Url,
                        IsMain = true
                    };

                    user.Photos.Add(photo);

                    IdentityResult result = await userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem creating user" });
                    }
                }

                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = jwtGenerator.CreateToken(user),
                    Username = user.Photos.FirstOrDefault(item => item.IsMain)?.Url
                };
            }
        }
    }
}