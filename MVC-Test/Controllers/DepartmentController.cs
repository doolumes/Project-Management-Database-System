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
using System.Data;
using Npgsql;
using MVC_Test.Models;
using System.Threading.Tasks;
using System.Xml;


namespace Group6Application.Controllers
{

    public class DepartmentController : Controller
    {
        private static string _connectionString = "Server=20.150.147.106;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("Department")]
        public ActionResult Index()
        {
            string viewPath = "Views/Department/Index.cshtml";
            
            DepartmentView viewModel = new DepartmentView()
            {
                Departments = new List<DepartmentTemplate>()
            };
            
            // Add datatable
            DataTable datatable = Data.Departments();

            foreach (DataRow row in datatable.Rows)
            {
                DepartmentTemplate deptartment = new DepartmentTemplate()
                {
                    ID = (int)row["ID"],
                    Name = row["Name"].ToString(),
                    Number_of_Employees = (int)row["Number_of_Employees"],
                    SupervisorID = row["SupervisorID"].ToString()  // doesn't matter if this can be null or not
                };
                viewModel.Departments.Add(deptartment);
            }
            
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
            List<SelectListItem> employeeIDs = new List<SelectListItem>();

            
             foreach(DataRow row in Data.EmployeeIDs().Rows)
             {
                employeeIDs.Add(new SelectListItem() { Value =row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString())});
             }
             
            viewModel.EmployeeIDs = employeeIDs;

            return PartialView(viewPath, viewModel);
        }

        public ActionResult AddDepartmentDB(string Name, string SupervisorID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Department\"(\"Name\",\"Number_of_Employees\",\"SupervisorID\") VALUES (@Name,@Number_of_Employees,@SupervisorID);";
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
                    command.Parameters.AddWithValue("@Number_of_Employees", 0);
                    // Supervisor ID can be NULL, if it is replace it with DBnull
                    command.Parameters.AddWithValue("@SupervisorID", (string.IsNullOrEmpty(SupervisorID)) ? (object)DBNull.Value : SupervisorID);
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

        [Route("Department/View")]
        public ActionResult ViewDepartment()
        {
            string viewPath = "Views/Department/ViewDepartment.cshtml";

            if (string.IsNullOrEmpty( Request.Query["id"] ))
            {
                Response.Redirect("/Department"); // if no id passed, redirect back to department
                return null; 
            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.Department(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return null;
            }

            DepartmentTemplate viewModel = new DepartmentTemplate() { 
                ID = (int)datatable.Rows[0]["ID"],
                Name = datatable.Rows[0]["Name"].ToString(),
                Number_of_Employees = (int)datatable.Rows[0]["Number_of_Employees"],
                SupervisorID = datatable.Rows[0]["SupervisorID"].ToString(),  // doesn't matter if this can be null or not
                Employees = new(),
                Projects = new()
            };

            DataTable projects = Data.Projects(viewModel.ID);
            foreach (DataRow row in projects.Rows)
            {
                Project project = new()
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Status = row["Status"].ToString(),
                    //StartDate = Convert.ToDateTime(row["StartDate"]),
                    //EndDate = Convert.ToDateTime(row["EndDate"]),

                };

                viewModel.Projects.Add(project);
            }

            return View(viewPath, viewModel);
        }
    }
}
