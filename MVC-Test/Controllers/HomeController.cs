using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Group6Application.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using Npgsql;
using MVC_Test.Models;
using System.Threading.Tasks;
using System.Xml;


namespace Group6Application.Controllers
{

    public class HomeController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("/")]
        public ActionResult Index()
        {
            //If no cookies are provided, then redirect to login page
            if (string.IsNullOrEmpty(Request.Cookies["key"]) || string.IsNullOrEmpty(Request.Cookies["id"])) {
                Response.Redirect("/Login");
                return Json(null);
            }
            
            string currentUserUsername = Request.Cookies["username"];
            DataTable datatable = Data.getEmployeeFromUsername(currentUserUsername);
            EmployeeTemplate employeeTemplate = new EmployeeTemplate() {
                ID = (int)datatable.Rows[0]["ID"],
                FirstName = datatable.Rows[0]["FirstName"].ToString(),
                LastName = datatable.Rows[0]["LastName"].ToString(),
                Department_ID = (int)datatable.Rows[0]["DepartmentID"],
            };

            return View(employeeTemplate);


            /*
            int userID=2; // ADD LOGIN VALIDATION to get this
            string viewPath = "Views/Home/Index.cshtml";
            
            HomeView viewModel = new HomeView()
            {
                AssignedTasks = new List<TaskModel>()
            };
            
            // Add datatable
            DataTable datatable = Data.AssignedTasks(userID);

            foreach (DataRow row in datatable.Rows)
            {
                TaskModel task = new TaskModel()
                {
                    ID = (int)row["ID"],
                    Name = row["Name"].ToString(),
                };

                DataTable project = Data.getProjectID(task.ID);
                task.ProjectID = (int)project.Rows[0]["ID"];

                viewModel.AssignedTasks.Add(task);
            }
            
            return View(viewPath, viewModel);
            */
        }

        public static DataTable AssignedTasks(int employeeID) {
            return Data.AssignedTasks(employeeID);
        }
        public static string CheckpointNameFromID(int checkpointID) {
            return Data.getCheckpointFromID(checkpointID).Rows[0]["Name"].ToString();
        }

        public static string ProjectNameFromCheckpointID(int checkpointID) {
            return Data.getProjectFromCheckpointID(checkpointID).Rows[0]["Name"].ToString();
        }

    }
}
