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
        public class Query : IRequest<List<Activity>> { }


        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext context;
            private readonly ILogger<List> logger;

            public Handler(DataContext context, ILogger<List> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                //try
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        cancellationToken.ThrowIfCancellationRequested();
                //        await Task.Delay(1000, cancellationToken);
                //        logger.LogInformation($"Task {i} has completed");
                //    }
                //}
                //catch (Exception exception) when (exception is TaskCanceledException)
                //{
                //    logger.LogInformation("Task was cancelled");
                //}

                List<Activity> activities = await context.Activities.ToListAsync(cancellationToken);

                return activities;
            }
        }
    }
}