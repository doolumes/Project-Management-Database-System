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

    public class TimesheetController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("Timesheet")]
        public ActionResult Index()
        {

            var cookie = Request.Cookies["key"];
            if (String.IsNullOrEmpty(cookie))
            {
                Response.Redirect("/Login");
                return RedirectToAction("Login", "Login");
            }

            int workerID;

            var idcookie = Request.Cookies["id"];
            if (String.IsNullOrEmpty(idcookie))
            {
                Response.Redirect("/Login");
				return RedirectToAction("Login", "Login");

			}
			else {
                workerID = Convert.ToInt32(idcookie);
            }

            string viewPath = "Views/Timesheet/Index.cshtml";

            TimesheetView viewModel = new()
            {
                Timesheets = new List<Timesheet>()
            };

            List<SelectListItem> projectIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.ProjectIDs().Rows)
            {
                projectIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = row["Name"].ToString() });
            }

            viewModel.ProjectIDs = projectIDs;

            // Add datatable
            DataTable datatable = Data.Timesheets(workerID);

            foreach (DataRow row in datatable.Rows)
            {
                Timesheet timesheet = new Timesheet()
                {
                    EntryID = (int)row["EntryID"],
                    Date = Convert.ToDateTime(row["Date"]),
                    ProjectID = (int)row["ProjectID"],
                    HoursWorked = (double)row["HoursWorked"],
                    WorkerID = (int)row["WorkerID"],
                };
                viewModel.Timesheets.Add(timesheet);
            }

            return View(viewPath, viewModel);
        }

        [Route("Timesheet/View")]
        public ActionResult View()
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
				return RedirectToAction("PermissionError", "Permission");
			}

			if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Timesheet"); 
                return RedirectToAction("Index", "Timesheet");

            }

            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            string viewPath = "Views/Timesheet/View.cshtml";

            TimesheetDeleteView viewModel = new(){};



            // Add datatable
            DataTable datatable = Data.Timesheet(id);

            Timesheet timesheet = new Timesheet()
            {
                EntryID = (int)datatable.Rows[0]["EntryID"],
                Date = Convert.ToDateTime(datatable.Rows[0]["Date"]),
                ProjectID = (int)datatable.Rows[0]["ProjectID"],
                HoursWorked = (double)datatable.Rows[0]["HoursWorked"],
                WorkerID = (int)datatable.Rows[0]["WorkerID"],
            };
            viewModel.timesheet = timesheet;

            DataTable employeeName = Data.EmployeeName(timesheet.WorkerID);
            DataTable projectName = Data.ProjectName(timesheet.ProjectID);

            viewModel.WorkerFirstName = employeeName.Rows[0]["FirstName"].ToString();
            viewModel.WorkerLastName = employeeName.Rows[0]["LastName"].ToString();
            viewModel.Project = projectName.Rows[0]["Name"].ToString();

            return View(viewPath, viewModel);
        }
        public ActionResult AddTimesheetDB(DateTime Date, int ProjectID, double HoursWorked)
        {
            int workerID;

            var idcookie = Request.Cookies["id"];
            if (String.IsNullOrEmpty(idcookie))
            {
                Response.Redirect("/Login");
                return RedirectToAction("Login", "Login");
            }
            else
            {
                workerID = Convert.ToInt32(idcookie);
            }

            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Timesheet\"(\"Date\",\"HoursWorked\",\"ProjectID\", \"WorkerID\") VALUES (@Date,@HoursWorked,@ProjectID,@WorkerID);";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                //try
                //{
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Date", Date);
                    command.Parameters.AddWithValue("@HoursWorked", HoursWorked);
                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    command.Parameters.AddWithValue("@WorkerID", workerID);

                    command.ExecuteScalar(); // Automatically creates primary key, must set constraint on primary key to "Identity"

                    sqlTransaction.Commit();
                    submissionResult = true;
                //}
                try { }
                catch (Exception e)
                {
                    // error catch here
                    sqlTransaction.Rollback();
                    errorMessage = "We experienced an error while adding to database";
                }
                finally
                {
                    conn.Close();
                }
            };

            return Json(new { submissionResult = submissionResult, message = errorMessage });
        }

        public ActionResult DeleteTimesheetDB(int TimesheetID)
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
				return RedirectToAction("PermissionError", "Permission");

			}

			bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"UPDATE \"Timesheet\" Set \"deleted\"=@deleted;";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                //try
                //{
                command.CommandText = sqlQuery.ToString();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@deleted", true);

                command.ExecuteScalar(); // Automatically creates primary key, must set constraint on primary key to "Identity"

                sqlTransaction.Commit();
                submissionResult = true;
                //}
                try { }
                catch (Exception e)
                {
                    // error catch here
                    sqlTransaction.Rollback();
                    errorMessage = "We experienced an error while adding to database";
                }
                finally
                {
                    conn.Close();
                }
            };

            return Json(new { submissionResult = submissionResult, message = errorMessage });
        }
    }
}
