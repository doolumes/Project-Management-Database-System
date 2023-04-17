using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;
using MVC_Test.Models;

namespace Group6Application.Models
{
    public class ProjectCosts
    {
        public Timesheet timesheet = new();
        public EmployeeTemplate employee = new();
        public double cost { get; set; } // employee hourly rate * timesheet hours worked
    }

    public class TaskInformation
    {
        public TaskModel task = new();
        public Project project = new();
        public EmployeeTemplate employee = new();
    }
    public class DownloadTasksView
    {
        public int DepartmentID { get; set; }
        public List<TaskInformation> Tasks = new List<TaskInformation>();
        public List<SelectListItem> Departments = new();
    }

    public class ReportsProjectsView
    {
        public int ProjectID { get; set; }
        public double totalHours { get;set; }
        public double totalWages { get; set; }
        public double totalExpenses { get; set; }

        public List<SelectListItem> Projects = new();
        public List<ProjectCosts> costs = new();
        public List<Expense> expenses = new();
    }

    public class OverdueTasksView
    {
        public int DepartmentID { get; set; }
        public List<TaskInformation> Tasks = new List<TaskInformation>();
        public List<SelectListItem> Departments = new();
    }

}
