using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Test.Models;
using System.ComponentModel.DataAnnotations;
using Group6Application.Model;

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

    public class CheckpointViewModel {
        public List<SelectListItem> ProjectIDs = new();
    }
}
