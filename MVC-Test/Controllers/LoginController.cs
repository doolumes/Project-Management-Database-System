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
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;

namespace MVC_Test.Controllers
{
    public class LoginController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        public IActionResult Index()
        {
            string viewPath = "Views/Login/Index.cshtml";

            LoginView viewModel = new LoginView()
            {
                Acount = new List<Account>()
            };
            return View(viewPath, viewModel);
        }

        public ActionResult Verify(Account acc)
        {



            //string sqlQuery = "SELECT * FROM \"Authentication\" WHERE \"Username\" = '" + acc.Username + "' and \"Password\" = '" + acc.Password + "';";
            //NpgsqlConnection conn = new NpgsqlConnection(_connectionString);


            // SQL
            string sqlQuery = "SELECT * FROM \"Authentication\" WHERE \"Username\" = '" + acc.Username + "' and \"Password\" = '" + acc.Password + "';";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                command.CommandText = sqlQuery.ToString();


                NpgsqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    conn.Close();
                    Response.Redirect("/Department"); // if not found in database, redirect back to department
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    conn.Close();
                    Response.Redirect("/Department"); // if not found in database, redirect back to department
                    return RedirectToAction("Index", "Department");
                }              
            }
        }
    }
}



