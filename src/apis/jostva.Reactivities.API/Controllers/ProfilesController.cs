using jostva.Reactivities.application.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace jostva.Reactivities.API.Controllers
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get(string username)
        {
            return await Mediator.Send(new Details.Query { Username = username });
        }
    }
}