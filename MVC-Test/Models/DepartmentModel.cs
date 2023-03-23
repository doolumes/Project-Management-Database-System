using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;

namespace Group6Application.Models
{
    public class DepartmentTemplate
    {
        public int ID { get; set; }
        public int? Number_of_Employees { get; set; }
        public string? Name { get; set; }
        public string? SupervisorID { get; set; }

        public List<Project> Projects = new List<Project>();
        public List <EmployeeTemplate> Employees = new List<EmployeeTemplate>();

    }
    public class DepartmentView
    {
        public List<DepartmentTemplate> Departments = new List<DepartmentTemplate>();
        public List<SelectListItem> EmployeeIDs { get; set; }
    }
  
}
