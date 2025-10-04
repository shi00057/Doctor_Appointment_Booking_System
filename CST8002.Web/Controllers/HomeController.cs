using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CST8002.Web.Models;

namespace CST8002.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
