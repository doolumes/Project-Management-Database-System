namespace MVC_Test.Models
{
    public class CheckpointModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectID { get; set; }
        public string Status { get; set; }

        public List<TaskModel> Tasks { get; set; }
    }
}
