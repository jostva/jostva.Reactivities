#region usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace jostva.Reactivities.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private IMediator mediator;

        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetService<IMediator>());

    }
}