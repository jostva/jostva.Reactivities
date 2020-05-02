﻿using AutoMapper;
using jostva.Reactivities.application.Errors;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string Body { get; set; }

            public Guid ActivityId { get; set; }

            public string Username { get; set; }
        }


        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;


            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }


            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                Activity activity = await context.Activities.FindAsync(request.ActivityId);
                if (activity == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });
                }

                AppUser user = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                Comment comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Body,
                    CreateAt = DateTime.Now
                };

                activity.Comments.Add(comment);

                bool success = await context.SaveChangesAsync() > 0;
                if (success)
                {
                    return mapper.Map<CommentDto>(comment);
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}