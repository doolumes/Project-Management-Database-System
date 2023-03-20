using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Group6Application.Model;

namespace MVC_Test.Controllers
{
    public class OutsourceController : Controller
    {
        [Route("Outsource")]

        public ActionResult Index()
        {
            string viewPath = "Views/Outsource/Index.cshtml";

            OutsourceView viewModel = new OutsourceView()
            {
                Outsource = new List<OutsourceTemplate>()
                {
                    new OutsourceTemplate
                    {
                        Name = "John",
                        Outsource_ID = 001,
                        Email = "john@gmail.com",
                        Phone_number = 8324560921,
                        Job = "security",
                        Task_ID = 003
                    },
                }
            };
            return View(viewPath, viewModel);
        }
    }
}
