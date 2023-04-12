using Group6Application.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group6Application.Models
{
    public class Timesheet { 
    
        public int EntryID { get; set; }
        public DateTime Date { get; set; }
        public double HoursWorked { get; set; }
        public int ProjectID { get; set; }
        public int WorkerID { get; set; }
    }

    public class TimesheetView
    {
        public List<Timesheet> Timesheets = new List<Timesheet>();
        public List<SelectListItem> ProjectIDs { get; set; }
    }

    public class TimesheetDeleteView
    {
        public Timesheet timesheet = new();
        public string? Project { get; set; }
        public string? WorkerFirstName { get; set; }
        public string? WorkerLastName { get; set; }

    }

}
