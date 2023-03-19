using System.ComponentModel.DataAnnotations;

namespace Group6Application.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? PhoneNumber { get; set; } = null!;
        public int? SupervisorID { get; set; } = null;
        public int? DepartmentID { get; set; } = null;
        public int? StartDate { get; set; } = null;
        public double? Wage { get; set; } = null;

        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Title { get; set; } = null!;


    }
}
