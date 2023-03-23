using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using MVC_Test.Models;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Group6Application.Controllers
{

    public class ExpenseController : Controller
    {
        private static string _connectionString = "Server=20.150.147.106;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        public ActionResult Index(/*string ProjectID*/)
        { int ProjectID = 1;// Development only, ^ add back for production



            string viewPath = "Views/Expense/_Index.cshtml";

            ExpenseView viewModel = new ExpenseView()
            {
                Expenses = new List<Expense>()
            };

            // SQL
            string sqlQuery = $"SELECT * FROM \"Expense\" WHERE ProjectID=@ProjectID;";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString)) { 
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Expense expense = new()
                        {
                            ExpenseID = (int)dataReader["ExpenseID"],
                            Name = dataReader["Name"].ToString(),
                            Description = dataReader["Description"].ToString(),
                            Cost = dataReader["Cost"].ToString(),
                            ProjectID = (int)dataReader["ProjectID"],
                        };
                        // possible null values
                        
                        viewModel.Expenses.Add(expense);
                    }
                }
                catch
                {
                    // error catch here
                }
                finally
                {
                    conn.Close();
                }

            }

            return PartialView(viewPath, viewModel);
        }

        public ActionResult AddExpense(string Name, string Description, string Cost, int ProjectID)
        {
            bool submissionResult = false;
            string errorMessage = "";

            // SQL
            string sqlQuery = $"INSERT INTO \"Expense\"(\"Name\",\"Description\",\"Cost\",\"ProjectID\") VALUES (@Name,@Description,@Cost,@ProjectID);";
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
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    
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
