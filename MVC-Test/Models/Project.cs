using System.ComponentModel.DataAnnotations;

namespace Group6Application.Model
{
    public class Project
    {
        [Key]
        public int ID { get; set; }
        public int StartDate { get; set; }
        public int DueDate { get; set; }
        public int CheckPointID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
