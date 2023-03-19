using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Group6Application.Models
{
    public class DepartmentTemplate
    {
        public int? ID { get; set; }
        public int? Number_Of_Employees { get; set; }
        public string? Name { get; set; }
        public string? SupervisorID { get; set; }
    }
    public class DepartmentView
    {
        public List<DepartmentTemplate> Departments = new List<DepartmentTemplate>();
        public int? ID { get; set; }
    }
  
}
