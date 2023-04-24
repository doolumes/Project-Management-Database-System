using Microsoft.AspNetCore.Mvc;
using MVC_Test.Models;
using System.Data;
using System.Threading.Tasks;
using Group6Application;
using Npgsql;
using System.Globalization;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Group6Application.Models;
using Microsoft.VisualBasic;

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
                Group6Application.Models.Project project = new Group6Application.Models.Project
                {
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

        [Route("Department/Project/AddCheckpoint")]
        public ActionResult Add()
        {

            string viewPath = "Views/Checkpoint/Add.cshtml";
            CheckpointViewModel viewModel = new();


            string id_temp = Request.Query["projID"].ToString();
            int id = Convert.ToInt32(id_temp);

            DataTable datatable = Data.ProjectName(id);

            List<SelectListItem> projectIDS = new List<SelectListItem>();

            foreach (DataRow row in datatable.Rows)
            {
                if (row["ID"].ToString() == id.ToString())
                {
                    projectIDS.Add(new SelectListItem() { Value = row["ID"].ToString(), Text = (row["Name"].ToString()) });
                }
            }

            viewModel.ProjectIDs = projectIDS;

            if (datatable.Rows.Count == 0)
            {
                Response.Redirect("/Department"); // if not found in database, redirect back to department
                return RedirectToAction("Index", "Department");

            }
            return View(viewPath, viewModel);
        }

        public ActionResult AddCheckpointDB(string Name, string Description, DateTime StartDate, DateTime DueDate, string ProjectID, string Status) 
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ProjectID))
            {
                return Json(new { submissionResult = false, message = "Checkpoint Name/Project is required!" });
            }
            bool submissionResult = false;
            string errorMessage = "";

            string sqlCommand = $"INSERT INTO \"Checkpoint\"(\"Name\",\"Description\",\"StartDate\",\"DueDate\",\"ProjectID\",\"Status\") VALUES (@Name,@Description,@StartDate,@DueDate,@ProjectID,@Status);";
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlCommand.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Description", String.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description);
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@DueDate", DueDate);
                    command.Parameters.AddWithValue("@ProjectID", (String.IsNullOrEmpty(ProjectID)) ? (object)DBNull.Value : Int32.Parse(ProjectID));
                    command.Parameters.AddWithValue("@Status", "Incomplete");

                    command.ExecuteScalar(); // Automatically creates primary key, must set constraint on primary key to "Identity"

                    sqlTransaction.Commit();
                    submissionResult = true;
                }
               catch (Exception e)
                {
                    // error catch here
                    sqlTransaction.Rollback();
                    errorMessage = "We experienced an error while adding to database";
                }
                finally
                {
                    conn.Close();
                }
            };
            return Json(new { submissionResult = submissionResult, message = errorMessage });
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
