using AutoMapper;
using jostva.Reactivities.application.Errors;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Activities
{
    public class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }
        }



        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }


            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                Activity activity = await context.Activities.FindAsync(request.Id);

                if (activity == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not Found" });
                }

                ActivityDto activityToReturn = mapper.Map<Activity, ActivityDto>(activity);

                return activityToReturn;
            }
        }
    }
}