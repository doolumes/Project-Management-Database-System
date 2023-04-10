using Group6Application.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group6Application.Models
{
    public class Timesheet
    {
        // Date
        // Hours Worked
        // Entry ID
        // Project ID
        // Worker ID

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
}
