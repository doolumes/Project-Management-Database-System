using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
using System.Reflection.Metadata;

namespace Group6Application.Controllers
{
	public class LoginController : Controller
	{
		private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
		[Route("Login")]
		public ActionResult Login()
		{
			string viewPath = "Views/Login/login.cshtml";
			return View(viewPath);
		}
		[Route("Verify")]
		public ActionResult Verify(string Username, string Password)
		{
			string username = Username;
			string password = Password;
			DataTable datatable = Data.login(username, password);
			if (datatable.Rows.Count == 0)
			{
				Response.Redirect("/Login"); // if no id passed, redirect back to department
				return RedirectToAction("Login", "Login");
			}
			else
			{
				Response.Redirect("/Employee"); // if no id passed, redirect back to department
				return RedirectToAction("Index", "Employee");
			}
		}

		[Route("Login/Register")]
		public ActionResult Register()
		{
			string viewPath = "Views/Login/register.cshtml";
			return View(viewPath);
		}
		public ActionResult Register_DB(string Username, string Password)
		{
			bool submissionResult = false;
			string errorMessage = "";
			string sqlQuery = $"INSERT INTO \"Authentication\"(\"Username\",\"Password\") VALUES (@Username,@Password);";
			using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
			{
				conn.Open();
				NpgsqlCommand command = new NpgsqlCommand("", conn);
				NpgsqlTransaction sqlTransaction;
				sqlTransaction = conn.BeginTransaction();
				command.Transaction = sqlTransaction;

				command.CommandText = sqlQuery.ToString();
				command.Parameters.Clear();

				command.Parameters.AddWithValue("@Username", Username);
				command.Parameters.AddWithValue("@Password", Password);
				command.ExecuteNonQuery();
				sqlTransaction.Commit();
				submissionResult = true;

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
	}
}



