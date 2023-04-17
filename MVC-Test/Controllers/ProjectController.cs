using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Group6Application.Model;
using Group6Application;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using Npgsql;
using MVC_Test.Models;
using System.Threading.Tasks;
using System.Xml;

namespace MVC_Test.Controllers
{
    public class ProjectController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        [Route("Project")]
        public IActionResult Index()
        {

            string viewPath = "Views/Project/Index.cshtml";

            ProjectView viewModel = new ProjectView()
            {
                Projects = new List<Project>()
            };

            //Add datatable
            DataTable datatable = Data.Projects();
            foreach (DataRow row in datatable.Rows) {
                Project project = new Project()
                {
                    ID = (int)row["ID"],
                    Name = row["Name"].ToString(),
                    Status = row["Status"].ToString(),
                    Description = row["Description"].ToString(),
                };
                project.CheckPointID = 0;
                project.EmployeeID = 0;
            }

            return View(viewPath, viewModel);
        }

        [Route("Department/AddProject")]

        public ActionResult AddProject()
        {
            /*var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
                return RedirectToAction("PermissionError", "Permission");
            }*/

            string viewPath = "Views/Project/AddProject.cshtml";
            ProjectView viewModel = new();


           string id_temp = Request.Query["deptID"].ToString();
           int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.Department(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }

            List<SelectListItem> DepartmentID = new List<SelectListItem>();

            foreach (DataRow row in Data.DepartmentID_data().Rows)
            {
                if (row["ID"].ToString() == id.ToString())
                {
                    DepartmentID.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["Name"].ToString()) });
                }
            }

            viewModel.DepartmentIDs = DepartmentID;

            List<SelectListItem> employeeIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }

            viewModel.EmployeeIDs = employeeIDs;

            List<SelectListItem> clientIDs  = new List<SelectListItem>();

            foreach (DataRow row in Data.ClientIDs().Rows)
            {
                clientIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["CompanyName"].ToString()) });
            }

            viewModel.ClientIDs = clientIDs;
            return PartialView(viewPath, viewModel);
        }


        public ActionResult AddProjectDB(string Name, string SupervisorID, string DepartmentID, String StartDate, String EndDate, String Description, String Status, String ClientID, float Budget, float Cost, String Hours)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Project\"(\"Name\",\"SupervisorID\",\"DepartmentID\",\"Description\",\"Status\",\"ClientID\",\"Budget\",\"Cost\",\"Hours\") VALUES (@Name,@SupervisorID,@DepartmentID,@Description,@Status,@ClientID, @Budget,@Cost,@Hours);";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@SupervisorID", (String.IsNullOrEmpty(SupervisorID)) ? (object)DBNull.Value : Int32.Parse(SupervisorID));
                    command.Parameters.AddWithValue("@DepartmentID", (String.IsNullOrEmpty(DepartmentID)) ? (object)DBNull.Value : Int32.Parse(DepartmentID));
                    command.Parameters.AddWithValue("@Description", String.IsNullOrEmpty(Description) ? DBNull.Value : Description);
                    command.Parameters.AddWithValue("@Status", "Not Started");
                    command.Parameters.AddWithValue("@ClientID", (String.IsNullOrEmpty(ClientID)) ? (object)DBNull.Value : Int32.Parse(ClientID));
                    command.Parameters.AddWithValue("@Budget", Budget);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@Hours", 0);

                    command.ExecuteScalar(); // Automatically creates primary key, must set constraint on primary key to "Identity"

                    sqlTransaction.Commit();
                    submissionResult = true;
                }
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


