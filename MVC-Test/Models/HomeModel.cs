using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;
using MVC_Test.Models;


namespace Group6Application.Models
{
    public class HomeView
    {
        public List<TaskModel> AssignedTasks = new List<TaskModel>();
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }

}
