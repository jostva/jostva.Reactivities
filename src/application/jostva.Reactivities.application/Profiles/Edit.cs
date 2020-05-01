using FluentValidation;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Profiles
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.context = context;
            }


            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                AppUser user = await context.Users.SingleOrDefaultAsync(x => x.UserName == userAccessor.GetCurrentUsername());

                user.DisplayName = request.DisplayName ?? user.DisplayName;
                user.Bio = request.Bio ?? user.Bio;

                bool success = await context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}