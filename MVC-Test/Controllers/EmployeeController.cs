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
        public ActionResult ViewEmplyee(int id)
        {
            string viewPath = "Views/Employee/View.cshtml";

            EmployeeView viewModel = new EmployeeView()
            {
                Employee = new List<EmployeeTemplate>()
            };

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                string sqlQuery = $"SELECT * FROM \"Employee\" WHERE \"ID\" = @id;";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader dataReader = command.ExecuteReader();
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
            return View(viewPath, viewModel);
        }

        [Route("Employee/Add")]
        public ActionResult AddEmplyee(EmployeeTemplate add)
        {
            string viewPath = "Views/Employee/Add.cshtml";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                string sqlQuery = $"INSERT INTO \"Employee\"(\"ID\",\"Address\",\"Wage\",\"FirstName\",\"LastName\",\"DepartmentID\",\"PhoneNumber\",\"Email\",\"Title\",\"StartDate\",\"SupervisorID\") VALUES (@ID,@Address,@Wage,@FirstName,@LastName,@DepartmentID,@PhoneNumber,@Email,@Title,@StartDate,@SupervisorID);";
                command.Parameters.AddWithValue("@Id", add.ID);
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
            return View(viewPath);
        }

        [Route("Employee/Delete")]
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
        }
    }
}