using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Test.Models;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Group6Application.Controllers
{
    public class EmployeeController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        [Route("Employee")]
        public ActionResult Index()
        {
            string viewPath = "Views/Employee/Index.cshtml";

            EmployeeView viewModel = new EmployeeView()
            {
                Employee = new List<EmployeeTemplate>()
            };

            DataTable datatable = Data.Employees();

            foreach (DataRow row in datatable.Rows)
            {
                EmployeeTemplate employees = new EmployeeTemplate()
                {
                    ID = (int)row["ID"],
                    Address = row["Address"].ToString(),
                    Wage = (double)row["Wage"],
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                };
                if (row["DepartmentID"] != null && row["DepartmentID"] != DBNull.Value)
                {
                    employees.Department_ID = (int)row["DepartmentID"];
                }
                if (row["PhoneNumber"] != null && row["PhoneNumber"] != DBNull.Value)
                {
                    employees.Phone_Number = row["PhoneNumber"].ToString();
                }
                if (row["Email"] != null && row["Email"] != DBNull.Value)
                {
                    employees.Email = row["Email"].ToString();
                }
                if (row["Title"] != null && row["Title"] != DBNull.Value)
                {
                    employees.Title = row["Title"].ToString();
                }
                if (row["StartDate"] != null && row["StartDate"] != DBNull.Value)
                {
                    employees.Start_Date = (string)row["StartDate"];
                }
                if (row["SupervisorID"] != null && row["SupervisorID"] != DBNull.Value)
                {
                    employees.Supervisor_ID = (int)row["SupervisorID"];
                }
                viewModel.Employee.Add(employees);
            }
            return View(viewPath, viewModel);
        }

       
        [Route("Employee/View")]
        public ActionResult ViewEmplyee()
        {
            string viewPath = "Views/Employee/View.cshtml";

            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Employee"); // if no id passed, redirect back to department
                return RedirectToAction("Index", "Employee");
            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.Employees_id(id);
            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Employee"); // if no id passed, redirect back to department
                return RedirectToAction("Index", "Employee");
            }

            EmployeeTemplate employees = new EmployeeTemplate()
            {
                ID = (int)datatable.Rows[0]["ID"],
                Address = datatable.Rows[0]["Address"].ToString(),
                Wage = (double)datatable.Rows[0]["Wage"],
                FirstName = datatable.Rows[0]["FirstName"].ToString(),
                LastName = datatable.Rows[0]["LastName"].ToString(),
            };
            if (datatable.Rows[0]["DepartmentID"] != null && datatable.Rows[0]["DepartmentID"] != DBNull.Value)
            {
                employees.Department_ID = (int)datatable.Rows[0]["DepartmentID"];
            }
            if (datatable.Rows[0]["PhoneNumber"] != null && datatable.Rows[0]["PhoneNumber"] != DBNull.Value)
            {
                employees.Phone_Number = datatable.Rows[0]["PhoneNumber"].ToString();
            }
            if (datatable.Rows[0]["Email"] != null && datatable.Rows[0]["Email"] != DBNull.Value)
            {
                employees.Email = datatable.Rows[0]["Email"].ToString();
            }
            if (datatable.Rows[0]["Title"] != null && datatable.Rows[0]["Title"] != DBNull.Value)
            {
                employees.Title = datatable.Rows[0]["Title"].ToString();
            }
            if (datatable.Rows[0]["StartDate"] != null && datatable.Rows[0]["StartDate"] != DBNull.Value)
            {
                employees.Start_Date = (string)datatable.Rows[0]["StartDate"];
            }
            if (datatable.Rows[0]["SupervisorID"] != null && datatable.Rows[0]["SupervisorID"] != DBNull.Value)
            {
                employees.Supervisor_ID = (int)datatable.Rows[0]["SupervisorID"];
            }

        return View(viewPath, employees);
    }


        [Route("Employee/Add")]
        public ActionResult AddEmplyee()
        {
            string viewPath = "Views/Employee/Add.cshtml";
            EmployeeView viewModel = new();
            List<SelectListItem> DepartmentID = new List<SelectListItem>();


            foreach (DataRow row in Data.DepartmentID_data().Rows)
            {
                DepartmentID.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["Name"].ToString())});
            }

            viewModel.DepartmentIDs = DepartmentID;

            List<SelectListItem> employeeIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }

            viewModel.EmployeeIDs = employeeIDs;

            return PartialView(viewPath, viewModel);
        }

        public ActionResult AddEmplyeeDB(string Address, int Wage, string FirstName, string LastName, int DepartmentID, string PhoneNumber, string Email, string Title, string StartDate, int SupervisorID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Employee\"(\"Address\",\"Wage\",\"FirstName\",\"LastName\",\"DepartmentID\",\"PhoneNumber\",\"Email\",\"Title\",\"StartDate\",\"SupervisorID\") VALUES (@Address,@Wage,@FirstName,@LastName,@DepartmentID,@PhoneNumber,@Email,@Title,@StartDate,@SupervisorID);";
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

                    command.Parameters.AddWithValue("@Address", String.IsNullOrEmpty(Address) ? DBNull.Value : Address);
                    command.Parameters.AddWithValue("@Wage", Wage);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                    command.Parameters.AddWithValue("@PhoneNumber", NpgsqlTypes.NpgsqlDbType.Varchar, String.IsNullOrEmpty(PhoneNumber) ? DBNull.Value : PhoneNumber);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@StartDate", NpgsqlTypes.NpgsqlDbType.Varchar, StartDate);
                    command.Parameters.AddWithValue("@SupervisorID", SupervisorID==0? DBNull.Value: SupervisorID); // check for null
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

        [Route("Employee/Delete")]
        public ActionResult DeleteEmplyee(int id)
        {
            string viewPath = "Views/Employee/Delete.cshtml";
            EmployeeView viewModel = new();
            List<SelectListItem> employeeIDs = new List<SelectListItem>();


            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }

            viewModel.EmployeeIDs = employeeIDs;

            return PartialView(viewPath, viewModel);
        }
        public ActionResult DeleteEmplyeeDB(int id)
        {
            bool submissionResult = false;
            string errorMessage = "";
            string sqlQuery = $"DELETE FROM \"Employee\" WHERE \"ID\"=@id;";
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

        [Route("Employee/Update")]
        public ActionResult UpdateEmployee()
        {
            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Employee"); // if no id passed, redirect back to department
                return RedirectToAction("Index", "Employee");
            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.Employees_id(id);
            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Employee"); // if no id passed, redirect back to department
                return RedirectToAction("Index", "Employee");
            }

            EmployeeTemplate employees = new EmployeeTemplate()
            {
                ID = (int)datatable.Rows[0]["ID"],
                Address = datatable.Rows[0]["Address"].ToString(),
                Wage = (double)datatable.Rows[0]["Wage"],
                FirstName = datatable.Rows[0]["FirstName"].ToString(),
                LastName = datatable.Rows[0]["LastName"].ToString(),
            };
            if (datatable.Rows[0]["DepartmentID"] != null && datatable.Rows[0]["DepartmentID"] != DBNull.Value)
            {
                employees.Department_ID = (int)datatable.Rows[0]["DepartmentID"];
            }
            if (datatable.Rows[0]["PhoneNumber"] != null && datatable.Rows[0]["PhoneNumber"] != DBNull.Value)
            {
                employees.Phone_Number = datatable.Rows[0]["PhoneNumber"].ToString();
            }
            if (datatable.Rows[0]["Email"] != null && datatable.Rows[0]["Email"] != DBNull.Value)
            {
                employees.Email = datatable.Rows[0]["Email"].ToString();
            }
            if (datatable.Rows[0]["Title"] != null && datatable.Rows[0]["Title"] != DBNull.Value)
            {
                employees.Title = datatable.Rows[0]["Title"].ToString();
            }
            if (datatable.Rows[0]["StartDate"] != null && datatable.Rows[0]["StartDate"] != DBNull.Value)
            {
                employees.Start_Date = (string)datatable.Rows[0]["StartDate"];
            }
            if (datatable.Rows[0]["SupervisorID"] != null && datatable.Rows[0]["SupervisorID"] != DBNull.Value)
            {
                employees.Supervisor_ID = (int)datatable.Rows[0]["SupervisorID"];
            }

            string viewPath = "Views/Employee/update.cshtml";
            UpdateEmplyeeView viewModel = new() {Employee_up = employees};
            List<SelectListItem> DepartmentID = new List<SelectListItem>();
            List<SelectListItem> employeeIDs = new List<SelectListItem>();

            foreach (DataRow row in Data.DepartmentID_data().Rows)
            {
                DepartmentID.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["Name"].ToString()) });
            }
            viewModel.DepartmentIDs = DepartmentID;

            foreach (DataRow row in Data.EmployeeIDs().Rows)
            {
                employeeIDs.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["FirstName"].ToString() + ' ' + row["LastName"].ToString()) });
            }
            viewModel.EmployeeIDs = employeeIDs;

            return PartialView(viewPath, viewModel);
        }

        public ActionResult UpdateEmplyeeDB(int id, string Address, int Wage, string FirstName, string LastName, int DepartmentID, string PhoneNumber, string Email, string Title, string StartDate, string SupervisorID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"UPDATE \"Employee\" SET \"Address\"=@Address,\"Wage\"=@Wage,\"FirstName\"=@FirstName,\"LastName\"=@LastName,\"DepartmentID\"=@DepartmentID,\"PhoneNumber\"=@PhoneNumber,\"Email\"=@Email,\"Title\"=@Title,\"StartDate\"=@StartDate,\"SupervisorID\"=@SupervisorID WHERE \"ID\"=@ID;;";
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

                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Address", String.IsNullOrEmpty(Address) ? DBNull.Value : Address);
                command.Parameters.AddWithValue("@Wage", Wage);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                command.Parameters.AddWithValue("@PhoneNumber", NpgsqlTypes.NpgsqlDbType.Varchar, String.IsNullOrEmpty(PhoneNumber) ? DBNull.Value : PhoneNumber);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@StartDate", NpgsqlTypes.NpgsqlDbType.Varchar, StartDate);
                command.Parameters.AddWithValue("@SupervisorID", (String.IsNullOrEmpty(SupervisorID)) ? (object)DBNull.Value : Int32.Parse(SupervisorID)); // check for null
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