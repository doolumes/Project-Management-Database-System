using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;


namespace Group6Application.Models
{
    public class EmployeeTemplate
    {
        public int? ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone_Number { get; set; }
        public int? Supervisor_ID { get; set; }
        public int? Department_ID { get; set; }
        public string? Start_Date { get; set; }
        public double? Wage { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public List<DepartmentTemplate> Department = new List<DepartmentTemplate>();
    }

    public class EmployeeView
    {
        public List<EmployeeTemplate> Employee = new List<EmployeeTemplate>();
        public List<SelectListItem> DepartmentIDs { get; set; }
        public List<SelectListItem> EmployeeIDs { get; set; }
    }
}
