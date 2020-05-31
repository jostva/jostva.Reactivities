using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace jostva.Reactivities.API.Controllers
{
    [AllowAnonymous]
    public class FallbackController : Controller
    {

        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Index.html"), "text/HTML");
        }
    }
}