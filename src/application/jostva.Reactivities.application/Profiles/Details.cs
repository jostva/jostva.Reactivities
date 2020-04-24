using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Profile> 
        {
            public string Username { get; set; }
        }


        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                AppUser user = await context.Users.SingleOrDefaultAsync(x=>x.UserName == request.Username);

                return new Profile()
                { 
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Image = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                    Photos = user.Photos,
                    Bio = user.Bio
                };
            }
        }
    }
}