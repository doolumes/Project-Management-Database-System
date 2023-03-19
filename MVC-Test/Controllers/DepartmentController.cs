using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;

namespace Group6Application.Controllers
{
    public class DepartmentController : Controller
    {
        [Route("Department")]
        public ActionResult Index()
        {
            string viewPath = "Views/Department/Index.cshtml";
            
            DepartmentView viewModel = new DepartmentView()
            {
                ID=10,
                Departments = new List<DepartmentTemplate>()
                {
                    
                    new DepartmentTemplate {
                    ID = 1,
                    Name= "Test Backend",
                    Number_Of_Employees=1,
                    SupervisorID="10"
                    },
                }
            };
            viewModel.ID = 10;

            return View(viewPath, viewModel);
        }

        public ActionResult AddDepartmentPage(string userRole)
        {
            if (userRole != "Manager" && userRole != "Supervisor") // only Manager and Supervisor can add a department, the rest get an error
            {
                bool submissionResult = false;
                string errorMessage = "User does not have permission to view this page";
                return Json(new { submissionResult = submissionResult, message = errorMessage });
            }

            string viewPath = "Pages/Department";
            DepartmentTemplate model = new();

            return PartialView(viewPath, model);
        }

        public ActionResult AddDepartment(string Name, string SupervisorID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // Connect to sql database here and input data

            return Json(new { submissionResult = submissionResult, message = errorMessage });
        }

    }
}
