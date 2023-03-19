using Microsoft.AspNetCore.Mvc;

namespace Group6Application.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
