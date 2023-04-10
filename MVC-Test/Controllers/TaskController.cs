    using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_Test.Models;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVC_Test.Controllers
{
    public class TaskController : Controller
    {

        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";
        public IActionResult Index(int taskID)
        {
            //Check if the provided ID exists

            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM \"Task\" WHERE \"ID\"=@id";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", taskID);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count <= 0) {
                return Content("This Task ID does not exist!");
            }
            connection.Close();
            return View(GetTaskModel(taskID));
        }

        public ActionResult Add() {
            return View();
        }

        public ActionResult AddTask(string taskName, string description = "", string checkPointID = null, string startDate = "", string dueDate = "", string assignee=null) {
            if (string.IsNullOrEmpty(taskName)) {
                return Json(new { submissionResult = false, message = "Task Name is required!" });
            }
            string sqlCommand = "INSERT INTO \"Task\" (\"Name\",\"Description\",\"CheckpointID\",\"Start\",\"Due\",\"Assignee\") VALUES (@name,@description,@checkpointid,@startdate,@duedate,@assignee) RETURNING \"ID\";";
            int ckID = 0;
            int assign = 0;
            if (string.IsNullOrEmpty(description)) {
                description = "NULL";
            }
            if (string.IsNullOrEmpty(startDate)) {
                startDate = "NULL";
            }
            if (string.IsNullOrEmpty(dueDate)) {
                dueDate = "NULL";
            }
            if (string.IsNullOrEmpty(checkPointID))
            {
                checkPointID = "NULL";
            }
            else {
                ckID = Int32.Parse(checkPointID);
            }
            if (string.IsNullOrEmpty(assignee))
            {
                assignee = "NULL";
            }
            else {
                assign = Convert.ToInt32(assignee);
            }
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue("@name", taskName);
            command.Parameters.AddWithValue("@description", description);

            if (ckID > 0)
            {
                command.Parameters.AddWithValue("@checkpointid", ckID);
            }
            else {
                command.Parameters.AddWithValue("@checkpointid", checkPointID);
            }
            if (assign > 0)
            {
                command.Parameters.AddWithValue("@assignee", assign);
            }
            else {
                command.Parameters.AddWithValue("@assignee", assignee);
            }
            if (startDate == "NULL")
            {
                command.Parameters.AddWithValue("@startdate", DBNull.Value);
            }
            else {
                DateTime sdate = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                command.Parameters.AddWithValue("@startdate", sdate);
            }
            if (dueDate == "NULL")
            {
                command.Parameters.AddWithValue("@duedate", DBNull.Value);
            }
            else {
                DateTime ddate = DateTime.ParseExact(dueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                command.Parameters.AddWithValue("@duedate", ddate);
            }
            try {
                int insertedID = (int)command.ExecuteScalar();
                connection.Close();
                return Json(new { submissionResult = true, message = "Success", id = insertedID });
            }catch (NpgsqlException e)
            {
                connection.Close();
                return Json(new { submissionResult = false, message = "Failed" });
            }

        }

        //Get Task based on task id
        public TaskModel GetTaskModel(int taskID) {
            TaskModel taskModel = new TaskModel() {ID = taskID};

            string sqlQuery = $"SELECT * FROM \"Task\" WHERE \"ID\"=@tskid;";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand("", conn))
            {
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("tskid", taskID);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    taskModel.Name = dataReader["Name"].ToString();
                    taskModel.Status = dataReader["Status"].ToString();
                    taskModel.Description = dataReader["Description"].ToString();
                    taskModel.Assignee = (Int32)dataReader["Assignee"];
                    taskModel.CheckpointID = (Int32)dataReader["CheckpointID"];
                    taskModel.Start = dataReader["Start"] == DBNull.Value ? (DateTime?)null : (DateTime)dataReader["Start"];
                    taskModel.Due = dataReader["Due"] == DBNull.Value ? (DateTime?)null : (DateTime)dataReader["Due"];
                }
                
            }

            return taskModel;
        }

        //Get employee based on employee id
        public static EmployeeTemplate getAssignee(int assigneeID) {
            EmployeeTemplate employeeTemplate = new EmployeeTemplate() {
                ID = assigneeID,
            };
            string sqlQuery = "SELECT * FROM \"Employee\" WHERE \"ID\"=@empID;";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand("", conn))
            {
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("empID", assigneeID);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    employeeTemplate.FirstName = dataReader["FirstName"].ToString();
                    employeeTemplate.LastName = dataReader["LastName"].ToString();
                }
                
            }
            return employeeTemplate;
        }

        //Get checkpoint based on checkpoint id
        public static CheckpointModel getCheckpoint(int checkpointID)
        {
            CheckpointModel checkpointModel = new CheckpointModel() { ID = checkpointID };
            string sqlQuery = "SELECT * FROM \"Checkpoint\" WHERE \"ID\"=@chkID;";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (NpgsqlCommand command = new NpgsqlCommand("", conn)) {
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("chkID", checkpointID);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()) {
                    checkpointModel.Name = dataReader["Name"].ToString();
                }
                
            }
            return checkpointModel;
        }

        //Gets a list of all checkpoints
        public static List<int> getCheckpoints() {
            List<int> lst = new List<int>();
            string sqlQuery = "SELECT * FROM \"Checkpoint\";";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                lst.Add((int)dataReader["ID"]);
            }
            return lst;
        }

        //Gets a list of all Employees
        public static List<int> getAssignees() {
            List<int> lst = new List<int>();
            string sqlQuery = "SELECT * FROM \"Employee\";";
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while(dataReader.Read())
            {
                lst.Add((int)dataReader["ID"]);
            }
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
