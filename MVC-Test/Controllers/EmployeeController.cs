using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Group6Application.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace MVC_Test.Controllers
{
    public class EmployeeController : Controller
    {
        [Route("Employee")]

        public ActionResult Index()
        {
            string viewPath = "Views/Employee/Index.cshtml";

            EmployeeView viewModel = new EmployeeView()
            {
                Employee = new List<EmployeeTemplate>()
                {
                    new EmployeeTemplate
                    {
                        ID = 1,
                        FirstName = "John",
                        LastName = "Smith",
                        Middle_Initial = "M",
                        Phone_Number = 8324957912,
                        Supervisor_ID = 5,
                        Department_ID = 10,
                        Start_Date = "04/18/2000",
                        Wage = 53.62,
                        Email = "johnsmith@gmail.com",
                        Address = "4400 University Dr, Houston, TX, 77004",
                        Title = "IT"
                    },
                }
            };

            return View(viewPath, viewModel);
        }
    }
}