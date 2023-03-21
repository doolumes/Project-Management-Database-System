using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using System.Data;



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

            string connectionString = "Server=20.150.147.106;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
            SqlConnection sql = new SqlConnection(connectionString); ;
            sql.Open();
            //https://www.guru99.com/insert-update-delete-asp-net.html
            SqlCommand command;
            return View(viewPath, viewModel);
        }

        [Route("Department/Add")]

        public ActionResult AddDepartment()
        {
            /*
            string userRole = Request.Cookies["Name"].Value;
            if (userRole != "Manager" && userRole != "Supervisor") // only Manager and Supervisor can add a department, the rest get an error
            {
                bool submissionResult = false;
                string errorMessage = "User does not have permission to view this page";
                return Json(new { submissionResult = submissionResult, message = errorMessage });
            }
            */
            string viewPath = "Views/Department/AddDepartment.cshtml";
            DepartmentView viewModel = new();
            List<SelectListItem> supervisorIDs = new List<SelectListItem>();

            
             foreach( DataRow row in Data.SupervisorIDs().Rows)
             {
                supervisorIDs.Add(new SelectListItem() { Value =row["ID"].ToString(), Text = row["Name"].ToString() });
             }
             
            viewModel.SupervisorIDs = supervisorIDs;

            return PartialView(viewPath, viewModel);
        }

        public ActionResult AddDepartmentDB(string Name, string SupervisorID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // Connect to sql database here and input data

            return Json(new { submissionResult = submissionResult, message = errorMessage });
        }

    }
}
