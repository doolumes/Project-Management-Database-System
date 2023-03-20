using Microsoft.AspNetCore.Mvc;

namespace MVC_Test.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
