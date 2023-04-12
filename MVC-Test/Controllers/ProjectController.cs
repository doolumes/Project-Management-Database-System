using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace MVC_Test.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            string viewPath = "Views/Project/Index.cshtml";
            Project project = new Project()
            {
                ID = 1,
                Name = "TaskForce",
                Status = "Not Started",
                Description = "Project Database Management System",
            };

            return View(project);
        }
    }
}
