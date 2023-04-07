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
    }
}



