using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;
using MVC_Test.Models;

namespace Group6Application.Models
{
    public class DownloadTasksView
    {
        public int DepartmentID { get; set; }
        public List<TaskModel> Tasks = new List<TaskModel>();
        public List<SelectListItem> Departments = new();

    }

}
