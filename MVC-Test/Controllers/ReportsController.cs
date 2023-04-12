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
using System.Collections;


namespace Group6Application.Controllers
{

    public class ReportsController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("Reports")]
        public ActionResult Index()
        {
            string viewPath = "Views/Reports/Index.cshtml";
            return View(viewPath);
        }

        [Route("Reports/Tasks")]
        public ActionResult Tasks()
        {
            
            string viewPath = "Views/Reports/DownloadTasks.cshtml";
            DownloadTasksView viewModel = new();
            List<SelectListItem> departments = new List<SelectListItem>();

            
             foreach(DataRow row in Data.Departments().Rows)
             {
                departments.Add(new SelectListItem() { Value =row["ID"].ToString(), Text = row["Name"].ToString() });
             }
             
            viewModel.Departments = departments;

            return View(viewPath, viewModel);
        }

        public ActionResult AddTasks(int DepartmentID)
        {
            string viewPath = "Views/Reports/_Tasks.cshtml";


            DownloadTasksView viewModel = new();
            List<SelectListItem> departments = new List<SelectListItem>();


            foreach (DataRow row in Data.Departments().Rows)
            {
                departments.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = row["Name"].ToString() });
            }

            viewModel.Departments = departments;

            // Get task data
            DataTable tasks = Data.getTasksFromDepartment(DepartmentID);

            List<TaskModel> taskList = new ();
            foreach (DataRow row in tasks.Rows)
            {
                TaskModel task = new TaskModel() { 
                    ID = (int)row["ID"],
                };

                if (!String.IsNullOrEmpty(row["Name"].ToString()))
                {
                    task.Name = row["Name"].ToString();
                }

                if (!String.IsNullOrEmpty(row["Description"].ToString()))
                {
                    task.Description = row["Description"].ToString();
                }

                if (!String.IsNullOrEmpty(row["Start"].ToString()))
                {
                    task.Start = Convert.ToDateTime(row["Start"]);
                }

                if (!String.IsNullOrEmpty(row["Due"].ToString()))
                {
                    task.Due = Convert.ToDateTime(row["Due"]);
                }

                if (!String.IsNullOrEmpty(row["Assignee"].ToString()))
                {
                    task.Assignee = (int)row["Assignee"];
                }

                taskList.Add(task);
            }

            viewModel.Tasks = taskList;

            return PartialView(viewPath, viewModel);


        }


    }
}
