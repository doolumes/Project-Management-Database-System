using Microsoft.AspNetCore.Mvc;

namespace MVC_Test.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
