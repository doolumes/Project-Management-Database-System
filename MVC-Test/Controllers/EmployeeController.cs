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

        /*[Route("Employee/View")]
        public ActionResult ViewEmplyee()
        {
            string viewPath = "Views/Employee/View.cshtml";

            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                Response.Redirect("/Department"); // if no id passed, redirect back to department
                return null;
            }
            string id_temp = Request.Query["id"].ToString();
            int id = Convert.ToInt32(id_temp);

            EmployeeTemplate viewModel = new EmployeeTemplate();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                string sqlQuery = $"SELECT * FROM \"Employee\" WHERE \"ID\" = @id;";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    if (dataReader["DepartmentID"] != null && dataReader["DepartmentID"] != DBNull.Value)
                    {
                        viewModel.Department_ID = (int)dataReader["DepartmentID"];
                    }
                    if (dataReader["PhoneNumber"] != null && dataReader["PhoneNumber"] != DBNull.Value)
                    {
                        viewModel.Phone_Number = dataReader["PhoneNumber"].ToString();
                    }
                    if (dataReader["Email"] != null && dataReader["Email"] != DBNull.Value)
                    {
                        viewModel.Email = dataReader["Email"].ToString();
                    }
                    if (dataReader["Title"] != null && dataReader["Title"] != DBNull.Value)
                    {
                        viewModel.Title = dataReader["Title"].ToString();
                    }
                    if (dataReader["StartDate"] != null && dataReader["StartDate"] != DBNull.Value)
                    {
                        viewModel.Start_Date = (DateTime)dataReader["StartDate"];
                    }
                    if (dataReader["SupervisorID"] != null && dataReader["SupervisorID"] != DBNull.Value)
                    {
                        viewModel.Supervisor_ID = (int)dataReader["SupervisorID"];
                    }
                }
            }
            return View(viewPath, viewModel);
        }*/

       /*public void AddEmplyeeDB(EmployeeTemplate add)
        {
           
            string sqlQuery = $"INSERT INTO \"Employee\"(\"ID\",\"Address\",\"Wage\",\"FirstName\",\"LastName\",\"DepartmentID\",\"PhoneNumber\",\"Email\",\"Title\",\"StartDate\",\"SupervisorID\") VALUES (@ID,@Address,@Wage,@FirstName,@LastName,@DepartmentID,@PhoneNumber,@Email,@Title,@StartDate,@SupervisorID);";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand("", conn))
            {
                command.Parameters.AddWithValue("@Id",add.ID);
                command.Parameters.AddWithValue("@Address", add.Address);
                command.Parameters.AddWithValue("@Wage", add.Wage);
                command.Parameters.AddWithValue("@FirstName", add.FirstName);
                command.Parameters.AddWithValue("@LastName", add.LastName);
                command.Parameters.AddWithValue("@DepartmentID", add.Department_ID);
                command.Parameters.AddWithValue("@PhoneNumber", add.Phone_Number);
                command.Parameters.AddWithValue("@Email", add.Email);
                command.Parameters.AddWithValue("@Title", add.Title);
                command.Parameters.AddWithValue("@StartDate", add.Start_Date);
                command.Parameters.AddWithValue("@SupervisorID", add.Supervisor_ID);
                command.ExecuteNonQuery();
            }
        }*/
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

                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Wage", Wage);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                    command.Parameters.AddWithValue("@PhoneNumber", NpgsqlTypes.NpgsqlDbType.Varchar, PhoneNumber);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Title", String.IsNullOrEmpty(Title)?DBNull.Value: Title);
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

        /*[Route("Employee/Delete")]
        public ActionResult DeleteEmplyee(int id)
        {
            string viewPath = "Views/Employee/Delete.cshtml";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                string sqlQuery = $"DELETE FROM \"Employee\" WHERE \"ID\" = @id;";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            return View(viewPath);
        }*/
    }
}