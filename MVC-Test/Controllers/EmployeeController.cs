using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            string sqlQuery = $"SELECT * FROM \"Employee\";";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand("", conn))
            {

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        EmployeeTemplate employees = new EmployeeTemplate()
                        {
                            ID = (int)dataReader["ID"],
                            Address = dataReader["Address"].ToString(),
                            Wage = (double)dataReader["Wage"],
                            FirstName = dataReader["FirstName"].ToString(),
                            LastName = dataReader["LastName"].ToString(),
                        };
                        if (dataReader["DepartmentID"] != null && dataReader["DepartmentID"] != DBNull.Value)
                        {
                            employees.Department_ID = (int)dataReader["DepartmentID"];
                        }
                        if (dataReader["PhoneNumber"] != null && dataReader["PhoneNumber"] != DBNull.Value)
                        {
                            employees.Phone_Number = dataReader["PhoneNumber"].ToString();
                        }
                        if (dataReader["Email"] != null && dataReader["Email"] != DBNull.Value)
                        {
                            employees.Email = dataReader["Email"].ToString();
                        }
                        if (dataReader["Title"] != null && dataReader["Title"] != DBNull.Value)
                        {
                            employees.Title = dataReader["Title"].ToString();
                        }
                        if (dataReader["StartDate"] != null && dataReader["StartDate"] != DBNull.Value)
                        {
                            employees.Start_Date = (DateTime)dataReader["StartDate"];
                        }
                        if (dataReader["SupervisorID"] != null && dataReader["SupervisorID"] != DBNull.Value)
                        {
                            employees.Supervisor_ID = (int)dataReader["SupervisorID"];
                        }
                        viewModel.Employee.Add(employees);
                    }
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return View(viewPath, viewModel);
        }

        [Route("Employee/View")]
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
        }

        [Route("Employee/Add")]
        public ActionResult AddEmplyee(int ID, string FirstName, string LastName, string Phone_Number, int Supervisor_ID, int Department_ID, DateTime Start_Date, double Wage, string Email, string Address, string Title)
        {
            string viewPath = "Views/Employee/Add.cshtml";
            string sqlQuery = $"INSERT INTO \"Employee\"(\"ID\",\"Address\",\"Wage\",\"FirstName\",\"LastName\",\"DepartmentID\",\"PhoneNumber\",\"Email\",\"Title\",\"StartDate\",\"SupervisorID\") VALUES (@ID,@Address,@Wage,@FirstName,@LastName,@DepartmentID,@PhoneNumber,@Email,@Title,@StartDate,@SupervisorID);";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn))
            {
                command.Parameters.AddWithValue("@Id",ID);
                command.Parameters.AddWithValue("@Address", (string.IsNullOrEmpty(Address)) ? (object)DBNull.Value : Address);
                command.Parameters.AddWithValue("@Wage", Wage);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@DepartmentID", Department_ID);
                command.Parameters.AddWithValue("@PhoneNumber", (string.IsNullOrEmpty(Phone_Number)) ? (object)DBNull.Value : Phone_Number);
                command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(Email)) ? (object)DBNull.Value : Email);
                command.Parameters.AddWithValue("@Title", (string.IsNullOrEmpty(Title)) ? (object)DBNull.Value : Title);
                command.Parameters.AddWithValue("@StartDate", Start_Date);
                command.Parameters.AddWithValue("@SupervisorID", Supervisor_ID);
                command.ExecuteNonQuery();
            }
            return View(viewPath);
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