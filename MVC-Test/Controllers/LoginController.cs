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
    public class LoginController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        [Route("Login")]
        public ActionResult Login()
        {
            string viewPath = "Views/Login/login.cshtml";
            return View(viewPath);
        }

        void connectionString()
        {
            con.ConnectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        }
		[Route("Verify")]
		public ActionResult Verify(Account acc)
        {
			string viewPath = "Views/Employee/Index.cshtml";
			string viewPath2 = "Views/Login/login.cshtml";
			connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM \"Authentication\" WHERE \"Username\"='"+acc.Username+"' AND \"Password\"='"+acc.Password+"' ";
            dr = com.ExecuteReader();
            if(dr.Read())
            {
				con.Close();
                return View(viewPath);
            }
            else
            {
				con.Close();
                return View(viewPath2);
            }
        }
    }
}