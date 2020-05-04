using AutoMapper;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Activities
{
    public class List
    {
        public class ActivitiesEnvelope
        {
            public List<ActivityDto> Activities { get; set; }

            public int ActivityCount { get; set; }
        }


        public class Query : IRequest<ActivitiesEnvelope>
        {
            public Query(int? limit, int? offset, bool isGoing, bool isHost, DateTime? startDate)
            {
                Limit = limit;
                Offset = offset;
                IsGoing = isGoing;
                IsHost = isHost;
                StartDate = startDate ?? DateTime.Now;
            }

            public int? Limit { get; set; }

            public int? Offset { get; set; }

            public bool IsGoing { get; set; }

            public bool IsHost { get; set; }

            public DateTime? StartDate { get; set; }
        }


        public class Handler : IRequestHandler<Query, ActivitiesEnvelope>
        {
            private readonly DataContext context;
            private readonly ILogger<List> logger;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, ILogger<List> logger, IMapper mapper, IUserAccessor userAccessor)
            {
                this.context = context;
                this.logger = logger;
                this.mapper = mapper;
                this.userAccessor = userAccessor;
            }

            public async Task<ActivitiesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<Activity> queryable = context.Activities
                                                        .Where(x => x.Date >= request.StartDate)
                                                        .OrderBy(x => x.Date)
                                                        .AsQueryable();

                if (request.IsGoing && !request.IsHost)
                {
                    queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName == userAccessor.GetCurrentUsername()));
                }

                if (request.IsHost &&!request.IsGoing)
                {
                    queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName == userAccessor.GetCurrentUsername() && a.IsHost));
                }

                List<Activity> activities = await queryable.Skip(request.Offset ?? 0)
                                                           .Take(request.Limit ?? 3)
                                                           .ToListAsync();

                return new ActivitiesEnvelope()
                {
                    Activities = mapper.Map<List<Activity>, List<ActivityDto>>(activities),
                    ActivityCount = queryable.Count()
                };
            }
        }
    }
}