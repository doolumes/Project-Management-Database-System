using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_Test.Models;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MVC_Test.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index(int taskID)
        {
            return View(GetTaskModel(taskID));
        }

        public TaskModel GetTaskModel(int taskID) {
            //TODO: Add SQL command to pull in the info from the SQL server
            TaskModel taskModel = new TaskModel() {
                ID = taskID,
                Name = "Task 1",
                Description = "Task 1 Description",
                Status = "In Progress",
                Assignee = 1,
                CheckpointID = 1,
                Start = DateTime.Now,
                Due = DateTime.Now.AddDays(30)
            };
            return taskModel;
        }

        public static EmployeeTemplate getAssignee(int assigneeID) {
            //TODO: Add SQL command to pull in employee info based on ID
            EmployeeTemplate employeeTemplate = new EmployeeTemplate() {
                ID = assigneeID,
            };
            if (assigneeID == 1)
            {
                employeeTemplate.FirstName = "John";
                employeeTemplate.LastName = "Smith";
            }
            else if (assigneeID == 2)
            {
                employeeTemplate.FirstName = "Dawson";
                employeeTemplate.LastName = "Hillebrand";
            }
            else if (assigneeID == 3)
            {
                employeeTemplate.FirstName = "Jane";
                employeeTemplate.LastName = "Louis";
            }
            else {
                employeeTemplate.FirstName = "Amanda";
                employeeTemplate.LastName = "Rogers";
            }
            return employeeTemplate;
        }

        public static CheckpointModel getCheckpoint(int checkpointID)
        {
            //TODO: ADD SQL Command to pull checkpint infor from ID
            CheckpointModel checkpointModel = new CheckpointModel() { ID = checkpointID };
            if (checkpointID == 1)
            {
                checkpointModel.Name = "Checkpoint 1";
            }
            else if (checkpointID == 2)
            {
                checkpointModel.Name = "Checkpoint 2";
            }
            else if (checkpointID == 3)
            {
                checkpointModel.Name = "Checkpoint 3";
            }
            return checkpointModel;
        }

        //Gets a list of all checkpoints
        public static List<int> getCheckpoints() {
            //TODO: Add SQL command to pull all avaialbale checkpoints
            List<int> lst = new List<int> { 1,2,3};
            return lst;
        }

        //Gets a list of all Employees
        public static List<int> getAssignees() { 
            //TODO: Add SQL command to pull all employees
            List<int> lst = new List<int> { 1,2,3};
            return lst;
        }

        [HttpPost]
        public static bool saveTaskChanges(FormCollection formCollection) {
            //TODO: Add SQL command to update task with changes
            string taskName = formCollection["taskName"];
            string taskDescription = formCollection["taskDescription"];
            return true;
        }

    }
}
