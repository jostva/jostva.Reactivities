using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using jostva.Reactivities.application.Errors;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Data;
using jostva.Reactivities.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace jostva.Reactivities.application.User
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }


        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(item => item.Email).NotEmpty();
                RuleFor(item => item.Password).NotEmpty();
            }
        }


        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly SignInManager<AppUser> signInManager;
            private readonly IJwtGenerator jwtGenerator;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    //  TODO: generate token
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Token = jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}