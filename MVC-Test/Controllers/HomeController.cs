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
            Response.Redirect("/Login"); // set login as home

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
        }

    }
}
