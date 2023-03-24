using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_Test.Models;
using Npgsql;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    }
}