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

    public class DepartmentController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("Department")]
        public IActionResult Index()
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
                DepartmentTemplate department = new DepartmentTemplate()
                {
                    ID = (int)row["ID"],
                    Name = row["Name"].ToString(),
                    SupervisorID = row["SupervisorID"].ToString(),  // doesn't matter if this can be null or not
                };
                department.Number_of_Employees = Data.Employees(department.ID).Rows.Count;
                viewModel.Departments.Add(department);
            }
            
            return View(viewPath, viewModel);
        }

        [Route("Department/Add")]
        public ActionResult AddDepartment()
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
				return RedirectToAction("PermissionError", "Permission");
			}

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
                    command.Parameters.AddWithValue("@SupervisorID", (String.IsNullOrEmpty(SupervisorID)) ? (object)DBNull.Value : Int32.Parse(SupervisorID));
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

        [Route("Department/Delete")]
        public ActionResult DeleteDepartment()
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
				return RedirectToAction("PermissionError", "Permission");
			}

			string viewPath = "Views/Department/DeleteDepartment.cshtml";

            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }

            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);


            DataTable datatable = Data.Department(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");
            }

            DepartmentTemplate viewModel = new DepartmentTemplate()
            {
                ID = (int)datatable.Rows[0]["ID"],
                Name = datatable.Rows[0]["Name"].ToString(),
                Number_of_Employees = 0,
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

            DataTable employees = Data.Employees(viewModel.ID);
            foreach (DataRow row in employees.Rows)
            {
                EmployeeTemplate employee = new()
                {
                    ID = Convert.ToInt32(row["ID"]),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                };

                viewModel.Employees.Add(employee);
                viewModel.Number_of_Employees++;
            }

            if (viewModel.Employees.Count == 0 && viewModel.Projects.Count == 0)
                viewModel.NoDependencies = true;
            else viewModel.NoDependencies = false;


            return PartialView(viewPath, viewModel);
        }

        public ActionResult DeleteDepartmentDB(int id)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"UPDATE \"Department\" SET\"deleted\"=@deleted, \"SupervisorID\"=@SupervisorID WHERE \"ID\"=@id;";
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
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@deleted", true);
                    command.Parameters.AddWithValue("@SupervisorID", DBNull.Value);

                    command.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    submissionResult = true;
                //}
                try { }
                catch (Exception e)
                {
                    // error catch here
                    sqlTransaction.Rollback();
                    errorMessage = "We experienced an error while accessing the database";
                }
                finally
                {
                    conn.Close();
                }
            };

            return Json(new { submissionResult = submissionResult, message = errorMessage });
        }

        [Route("Department/Update")]
        public ActionResult UpdateDepartment()
        {
            var cookie = Request.Cookies["key"];
            if (cookie != "Manager")
            {
                Response.Redirect("/Permission");
				return RedirectToAction("PermissionError", "Permission");
			}

			if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Department");
                return RedirectToAction("Index", "Department");
            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable deptTable = Data.Department(id);

            if (deptTable.Rows.Count == 0) 
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");
            }

            DepartmentTemplate department = new DepartmentTemplate()
            {
                ID = (int)deptTable.Rows[0]["ID"],
                Name = deptTable.Rows[0]["Name"].ToString(),
                Number_of_Employees = 0,
                SupervisorID = deptTable.Rows[0]["SupervisorID"].ToString(),  // doesn't matter if this can be null or not
                Employees = new(),
                Projects = new()
            };

            string viewPath = "Views/Department/UpdateDepartment.cshtml";
            UpdateDepartmentView viewModel = new() { Department=department};
            List<SelectListItem> employeeIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }

            viewModel.EmployeeIDs = employeeIDs;

            return PartialView(viewPath, viewModel);
        }

        public ActionResult UpdateDepartmentDB(string Name, string SupervisorID, int id)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"UPDATE \"Department\" SET \"Name\"=@Name,\"SupervisorID\"=@SupervisorID WHERE \"ID\"=@ID;";
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
                    command.Parameters.AddWithValue("@ID", id);

                    // Supervisor ID can be NULL, if it is replace it with DBnull
                    command.Parameters.AddWithValue("@SupervisorID", (String.IsNullOrEmpty(SupervisorID)    ) ? (object)DBNull.Value : Int32.Parse(SupervisorID));
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


        [Route("Department/View")]
        public ActionResult ViewDepartment()
        {
            var cookie = Request.Cookies["key"];
            if (cookie == null)
            {
                Response.Redirect("/Login");
                return RedirectToAction("Login", "Login");
            }

            string viewPath = "Views/Department/ViewDepartment.cshtml";

            if (string.IsNullOrEmpty( Request.Query["id"] ))
            {
                Response.Redirect("/Department"); // if no id passed, redirect back to department
                return RedirectToAction("Index", "Department");


            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.Department(id);

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }

            DepartmentTemplate viewModel = new DepartmentTemplate() { 
                ID = (int)datatable.Rows[0]["ID"],
                Name = datatable.Rows[0]["Name"].ToString(),
                Number_of_Employees = 0,
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

            DataTable employees = Data.Employees(viewModel.ID);
            foreach (DataRow row in employees.Rows)
            {
                EmployeeTemplate employee = new()
                {
                    ID = Convert.ToInt32(row["ID"]),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                };

                viewModel.Employees.Add(employee);
                viewModel.Number_of_Employees++;
            }

            return View(viewPath, viewModel);
        }
    }
}
