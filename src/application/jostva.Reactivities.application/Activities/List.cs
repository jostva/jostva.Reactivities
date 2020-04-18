using AutoMapper;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Activities
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>> { }


        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly DataContext context;
            private readonly ILogger<List> logger;
            private readonly IMapper mapper;

            public Handler(DataContext context, ILogger<List> logger, IMapper mapper)
            {
                this.context = context;
                this.logger = logger;
                this.mapper = mapper;
            }

            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Activity> activities = await context.Activities.ToListAsync();

                return mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            }
        }
    }
}