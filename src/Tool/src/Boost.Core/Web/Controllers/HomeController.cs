using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Boost.Account;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Boost
{
    [Authorize]
    [Route("server")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }
    }
}
