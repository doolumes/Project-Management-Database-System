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
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;

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

                viewModel.Projects.Add(project);
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


        public ActionResult AddProjectDB(string Name, string SupervisorID, string DepartmentID, DateTime StartDate, DateTime EndDate, String Description, String Status, String ClientID, double Budget, double Cost, String Hours)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Project\"(\"Name\",\"SupervisorID\",\"DepartmentID\",\"Description\",\"Status\",\"ClientID\",\"Budget\",\"Cost\",\"Hours\",\"StartDate\",\"EndDate\") VALUES (@Name,@SupervisorID,@DepartmentID,@Description,@Status,@ClientID, @Budget,@Cost,@Hours,@StartDate,@EndDate);";
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
                    command.Parameters.AddWithValue("@Description", String.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description);
                    command.Parameters.AddWithValue("@Status", "Incomplete");
                    command.Parameters.AddWithValue("@ClientID", (String.IsNullOrEmpty(ClientID)) ? (object)DBNull.Value : Int32.Parse(ClientID));
                    command.Parameters.AddWithValue("@Budget", Budget);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@Hours", 0);
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);

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

        [Route("Department/ViewProject")]
        public ActionResult ViewProject()
        {
            var cookie = Request.Cookies["key"];
            if (String.IsNullOrEmpty(cookie))
            {
                Response.Redirect("/Login");
                return RedirectToAction("Login", "Login");
            }

            string viewPath = "Views/Project/View.cshtml";

            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Project"); // if no id passed, redirect back to department
                return RedirectToAction("ViewProject", "Project");


            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.ProjectName(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }

            Project viewModel = new Project(){
            };

            if (datatable.Rows[0]["ID"] != null && datatable.Rows[0]["ID"] != DBNull.Value)
            {
                viewModel.ID = (int)datatable.Rows[0]["ID"];
            }
            if (datatable.Rows[0]["Name"] != null && datatable.Rows[0]["Name"] != DBNull.Value)
            {
                viewModel.Name = datatable.Rows[0]["Name"].ToString();
            }
            if (datatable.Rows[0]["SupervisorID"] != null && datatable.Rows[0]["SupervisorID"] != DBNull.Value)
            {
                viewModel.SupervisorID = (int)datatable.Rows[0]["SupervisorID"]; // doesn't matter if this can be null or not
            }
            if (datatable.Rows[0]["DepartmentID"] != null && datatable.Rows[0]["DepartmentID"] != DBNull.Value)
            {
                viewModel.DepartmentID = (int)datatable.Rows[0]["DepartmentID"];
            }

            if (datatable.Rows[0]["StartDate"] != null && datatable.Rows[0]["StartDate"] != DBNull.Value)
            {
                //viewModel.StartDate = (DateTime)datatable.Rows[0]["StartDate"];
                viewModel.StartDate = Convert.ToDateTime(datatable.Rows[0]["StartDate"]);
            }
            if (datatable.Rows[0]["EndDate"] != null && datatable.Rows[0]["EndDate"] != DBNull.Value)
            {
                viewModel.EndDate = Convert.ToDateTime(datatable.Rows[0]["EndDate"]);
            }
            if (datatable.Rows[0]["ClientID"] != null && datatable.Rows[0]["ClientID"] != DBNull.Value)
            {
                viewModel.ClientID = (int)datatable.Rows[0]["ClientID"];
            }
            if (datatable.Rows[0]["Budget"] != null && datatable.Rows[0]["Budget"] != DBNull.Value)
            {
                viewModel.Budget = (double)datatable.Rows[0]["Budget"];
            }
            if (datatable.Rows[0]["Cost"] != null && datatable.Rows[0]["Cost"] != DBNull.Value)
            {
                viewModel.Cost = (double)datatable.Rows[0]["Cost"];
            }
            if (datatable.Rows[0]["Description"] != null && datatable.Rows[0]["Description"] != DBNull.Value)
            {
                viewModel.Description = datatable.Rows[0]["Description"].ToString();
            }
            if (datatable.Rows[0]["Status"] != null && datatable.Rows[0]["Status"] != DBNull.Value)
            {
                viewModel.Status = datatable.Rows[0]["Status"].ToString();
            }

            DataTable Checkpoints = Data.getCheckpointsFromProjectID(viewModel.ID);
            foreach (DataRow row in Checkpoints.Rows)
            {
                CheckpointModel checkpoints = new()
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Status = row["Status"].ToString(),
                    //StartDate = Convert.ToDateTime(row["StartDate"]),
                    //EndDate = Convert.ToDateTime(row["EndDate"]),

                };

                viewModel.Checkpoints.Add(checkpoints);
            }
            return View(viewPath, viewModel);
        }



        [Route("Department/UpdateProject")]

        public ActionResult UpdateProject()
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
                return RedirectToAction("PermissionError", "Permission");
            }

            string viewPath = "Views/Project/UpdateProject.cshtml";


            string id_temp = Request.Query["projID"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.ProjectName(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }


            Project viewModel = new Project()
            {
            };

            if (datatable.Rows[0]["ID"] != null && datatable.Rows[0]["ID"] != DBNull.Value)
            {
                viewModel.ID = (int)datatable.Rows[0]["ID"];
            }
            if (datatable.Rows[0]["Name"] != null && datatable.Rows[0]["Name"] != DBNull.Value)
            {
                viewModel.Name = datatable.Rows[0]["Name"].ToString();
            }
            if (datatable.Rows[0]["SupervisorID"] != null && datatable.Rows[0]["SupervisorID"] != DBNull.Value)
            {
                viewModel.SupervisorID = (int)datatable.Rows[0]["SupervisorID"]; // doesn't matter if this can be null or not
            }
            if (datatable.Rows[0]["DepartmentID"] != null && datatable.Rows[0]["DepartmentID"] != DBNull.Value)
            {
                viewModel.DepartmentID = (int)datatable.Rows[0]["DepartmentID"];
            }

            if (datatable.Rows[0]["StartDate"] != null && datatable.Rows[0]["StartDate"] != DBNull.Value)
            {
                //viewModel.StartDate = (DateTime)datatable.Rows[0]["StartDate"];
                viewModel.StartDate = Convert.ToDateTime(datatable.Rows[0]["StartDate"]);
            }
            if (datatable.Rows[0]["EndDate"] != null && datatable.Rows[0]["EndDate"] != DBNull.Value)
            {
                viewModel.EndDate = Convert.ToDateTime(datatable.Rows[0]["EndDate"]);
            }
            if (datatable.Rows[0]["ClientID"] != null && datatable.Rows[0]["ClientID"] != DBNull.Value)
            {
                viewModel.ClientID = (int)datatable.Rows[0]["ClientID"];
            }
            if (datatable.Rows[0]["Budget"] != null && datatable.Rows[0]["Budget"] != DBNull.Value)
            {
                viewModel.Budget = (double)datatable.Rows[0]["Budget"];
            }
            if (datatable.Rows[0]["Cost"] != null && datatable.Rows[0]["Cost"] != DBNull.Value)
            {
                viewModel.Cost = (double)datatable.Rows[0]["Cost"];
            }
            if (datatable.Rows[0]["Description"] != null && datatable.Rows[0]["Description"] != DBNull.Value)
            {
                viewModel.Description = datatable.Rows[0]["Description"].ToString();
            }
            if (datatable.Rows[0]["Status"] != null && datatable.Rows[0]["Status"] != DBNull.Value)
            {
                viewModel.Status = datatable.Rows[0]["Status"].ToString();
            }





            UpdateProjectView viewModel2 = new() { Project_up = viewModel };



            List<SelectListItem> DepartmentID = new List<SelectListItem>();

            foreach (DataRow row in Data.DepartmentID_data().Rows)
            {
                DepartmentID.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["Name"].ToString()) });
                
            }


            viewModel2.DepartmentIDs = DepartmentID;

            List<SelectListItem> employeeIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }

            viewModel2.EmployeeIDs = employeeIDs;

            List<SelectListItem> clientIDs = new List<SelectListItem>();

            foreach (DataRow row in Data.ClientIDs().Rows)
            {
                clientIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["CompanyName"].ToString()) });
            }

            viewModel2.ClientIDs = clientIDs;
            return View(viewPath, viewModel2);
        }


        public ActionResult UpdateProjectDB(int ID, string Name, string SupervisorID, string DepartmentID, DateTime StartDate, DateTime EndDate, String Description, String Status, String ClientID, double Budget, double Cost, String Hours)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"UPDATE \"Project\" SET \"Name\"=@Name,\"DepartmentID\"=@DepartmentID,\"SupervisorID\"=@SupervisorID,\"StartDate\"=@StartDate,\"EndDate\"=@EndDate,\"Description\"=@Description,\"Status\"=@Status,\"Budget\"=@Budget,\"Cost\"=@Cost WHERE \"ID\"=@ID;";
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
                    command.Parameters.AddWithValue("@ID", ID); // check for null
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@SupervisorID", (String.IsNullOrEmpty(SupervisorID)) ? (object)DBNull.Value : Int32.Parse(SupervisorID));
                    command.Parameters.AddWithValue("@DepartmentID", (String.IsNullOrEmpty(DepartmentID)) ? (object)DBNull.Value : Int32.Parse(DepartmentID));
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@ClientID", (String.IsNullOrEmpty(ClientID)) ? (object)DBNull.Value : Int32.Parse(ClientID));
                    command.Parameters.AddWithValue("@Budget", Budget);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@Hours", 0);
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    command.ExecuteNonQuery();

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


