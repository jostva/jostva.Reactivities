#region usings

using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace jostva.Reactivities.application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<User> { }


        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IJwtGenerator jwtGenerator;
            private readonly IUserAccessor userAccessor;

            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
                this.userAccessor = userAccessor;
            }


            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(userAccessor.GetCurrentUsername());

                return new User()
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Token = jwtGenerator.CreateToken(user),
                    Image = user.Photos.FirstOrDefault(item => item.IsMain)?.Url
                };
            }
        }
    }
}