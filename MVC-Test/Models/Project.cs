using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Test.Models;
using System.ComponentModel.DataAnnotations;
using Group6Application.Model;

namespace Group6Application.Models
{
    public class Project
    {
        [Key]
        public int ID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CheckPointID { get; set; }
        public int? EmployeeID { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public int? TotalHours { get; set; }
        public float? Cost { get; set; }
        public float? Budget { get; set; }
        public int? ClientID{ get; set; }
        public int SupervisorID { get; set; }
        public int DepartmentID { get; set; }

        public List<CheckpointModel> Checkpoints = new List<CheckpointModel>();
        public List<Expense> Expenses = new List<Expense>();
    }

    public class ProjectView
    {
        public List<Project> Projects = new List<Project>();
        public List<Expense> ExpenseIDs { get; set; }
        public List<SelectListItem> CheckpointIDs { get; set; }
        public List<SelectListItem> DepartmentIDs { get; set; }
        public List<SelectListItem> EmployeeIDs { get; set; }
    }

    public class UpdateProjectView
    {
        public Project Project = new();
        public List<SelectListItem> ExpenseIDs { get; set; }
        public List<SelectListItem> CheckpointIDs { get; set; }
    }

}
