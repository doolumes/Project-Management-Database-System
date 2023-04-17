using Microsoft.AspNetCore.Mvc;
using MVC_Test.Models;
using System.Data;
using System.Threading.Tasks;
using Group6Application;
using Npgsql;
using System.Globalization;

namespace MVC_Test.Controllers
{
    public class CheckpointController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        public IActionResult Index(int? checkpointID)
        {
            if (checkpointID is null)
            {
                return Content("Task ID or Checkpoint ID must be provided!");
            }
            CheckpointModel checkpointModel = new CheckpointModel();
            DataTable dataTable = Data.getCheckpointFromID((int)checkpointID);
            checkpointModel.ID = int.Parse(dataTable.Rows[0]["ID"].ToString());
            checkpointModel.Name = dataTable.Rows[0]["Name"].ToString();
            checkpointModel.Status = dataTable.Rows[0]["Status"].ToString();
            checkpointModel.Description = dataTable.Rows[0]["Description"].ToString();
            checkpointModel.StartDate = DateTime.Parse(dataTable.Rows[0]["StartDate"].ToString());
            checkpointModel.DueDate = DateTime.Parse(dataTable.Rows[0]["DueDate"].ToString());
            return View(checkpointModel);
        }

        public ActionResult SaveChanges(string checkpointID, string checkpointName, string checkpointDescription = "")
        {
            if (string.IsNullOrEmpty(checkpointName) || string.IsNullOrEmpty(checkpointID))
            {
                return Json(new { submissionResult = false, message = "Checkpoint Name is required!" });
            }

            string sqlCommand = "UPDATE \"Checkpoint\" SET \"Name\"=@name, \"Description\"=@description WHERE \"ID\"=@checkpointID RETURNING \"ID\";";
            int ckID = 0;
            int assign = 0;
            int chkID = Int32.Parse(checkpointID);
            if (string.IsNullOrEmpty(checkpointDescription))
            {
                checkpointDescription = "NULL";
            }
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue("@name", checkpointName);
            command.Parameters.AddWithValue("@description", checkpointDescription);
            command.Parameters.AddWithValue("@checkpointID", chkID);
            
            try
            {
                int insertedID = (int)command.ExecuteScalar();
                connection.Close();
                return Json(new { submissionResult = true, message = "Success", id = insertedID });
            }
            catch (NpgsqlException e)
            {
                connection.Close();
                return Json(new { submissionResult = false, message = "Failed" });
            }
        }
    }
}
