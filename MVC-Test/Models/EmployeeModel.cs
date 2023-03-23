using System;
using System.Collections.Generic;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
        public DateTime? Start_Date { get; set; }
        public double? Wage { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
    }

    public class EmployeeView
    {
        public List<EmployeeTemplate> Employee = new List<EmployeeTemplate>();
    }
}
