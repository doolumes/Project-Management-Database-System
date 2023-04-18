using Microsoft.AspNetCore.Mvc;
using MVC_Test.Models;
using System.Data;
using System.Threading.Tasks;
using Group6Application;
using Npgsql;
using System.Globalization;
using Group6Application.Models;

namespace MVC_Test.Controllers
{
    public class CheckpointController : Controller
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        public IActionResult Index(int? checkpointID, int? projectID)
        {
            if (checkpointID == null && projectID == null) {
                return View("Checkpoint ID or Project ID must be provided!");
            }
            if (checkpointID is null)
            {
                DataTable datatable = Data.getCheckpointsFromProjectID((int)projectID);
                DataTable datatable2 = Data.getProjectFromID((int)projectID);
                List<CheckpointModel> checkpoints = new List<CheckpointModel>();
                foreach (DataRow row in datatable.Rows)
                {
                    CheckpointModel model = new CheckpointModel() {
                        ID = int.Parse(row["ID"].ToString()),
                        Name = row["Name"].ToString(),
                        Status = row["Status"].ToString(),
                        StartDate = DateTime.Parse(row["StartDate"].ToString()),
                        DueDate = DateTime.Parse(row["DueDate"].ToString()),
                    };
                    checkpoints.Add(model);
                }
                Project project = new Project {
                    Name = datatable2.Rows[0]["Name"].ToString(),
                    ID = int.Parse(datatable2.Rows[0]["ID"].ToString()),
                    Checkpoints = checkpoints,
                };
                return View("~/Views/Checkpoint/CheckpointList.cshtml", project);
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
