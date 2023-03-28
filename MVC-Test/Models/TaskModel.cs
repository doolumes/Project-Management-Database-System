namespace MVC_Test.Models
{
    public class TaskModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Due { get; set; }
        public int CheckpointID { get; set; }
        public int Assignee { get; set; }
        public int ProjectID { get; set; }
    }
}
