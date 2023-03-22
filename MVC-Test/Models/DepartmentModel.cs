using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group6Application.Models
{
    public class DepartmentTemplate
    {
        public int? ID { get; set; }
        public int? Number_of_Employees { get; set; }
        public string? Name { get; set; }
        public string? SupervisorID { get; set; }
    }
    public class DepartmentView
    {
        public List<DepartmentTemplate> Departments = new List<DepartmentTemplate>();
        public List<SelectListItem> SupervisorIDs { get; set; }
    }
  
}
