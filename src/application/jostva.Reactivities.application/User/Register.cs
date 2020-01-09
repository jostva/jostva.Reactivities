#region usings

using FluentValidation;
using jostva.Reactivities.application.Errors;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.application.Validators;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace jostva.Reactivities.application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }


        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(item => item.DisplayName).NotEmpty();
                RuleFor(item => item.UserName).NotEmpty();
                RuleFor(item => item.Email).NotEmpty().EmailAddress();
                RuleFor(item => item.Password).Password();
            }
        }


        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext context;
            private readonly UserManager<AppUser> userManager;
            private readonly IJwtGenerator jwtGenerator;
            
            
            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
            }


            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.Where(item => item.Email == request.Email).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });
                }

                if (await context.Users.Where(item => item.UserName == request.UserName).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = "UserName already exists" });
                }

                AppUser user = new AppUser()
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                IdentityResult result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new User()
                    {
                        DisplayName = user.DisplayName,
                        Token = jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}