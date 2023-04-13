using Npgsql;
using System.Data;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Data.SqlClient;
using Npgsql;
using MVC_Test.Models;
using System.Threading.Tasks;
using System.Xml;
using Group6Application.Model;


namespace Group6Application
{
    public class Data
    {
        private static string _connectionString = "Server=20.124.84.12;Port=5432;Database=Group6-PMS;User Id=postgres;Password=KHf37p@&R2hf2l";

        // returns supervisor IDs for all departments
        public static DataTable EmployeeIDs()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT \"ID\", \"FirstName\", \"LastName\" FROM \"Employee\"";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }

        public static DataTable ProjectName(int projectID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT \"ID\", \"Name\" FROM \"Project\" WHERE \"ID\"=@projectID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@projectID", projectID);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }

        public static DataTable EmployeeName(int employeeID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT \"ID\", \"FirstName\", \"LastName\" FROM \"Employee\" WHERE \"ID\"=@employeeID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@employeeID", employeeID);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }

        public static DataTable ProjectIDs()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT \"ID\", \"Name\" FROM \"Project\"";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }

        // returns all information from department with id matching id
        public static DataTable Department(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Department\" WHERE \"ID\"=@ID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        // returns all information from all departments
        public static DataTable Departments()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Department\" WHERE \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }


        // returns all employees from given department
        public static DataTable Employees(int departmentID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Employee\" WHERE \"DepartmentID\"=@DeptID";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DeptID", departmentID);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }
        // returns all employees
        public static DataTable Employees()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Employee\"";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        // returns all employees from given department
        public static DataTable Projects(int departmentID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Project\" WHERE \"DepartmentID\"=@DeptID";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DeptID", departmentID);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }
        // returns all projects
        public static DataTable Projects()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Project\"";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        public static DataTable AssignedTasks(int userID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Task\" WHERE \"Assignee\"=@userID";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@userID", userID);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        public static DataTable getProjectID(int taskID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                /*
                 Select project."ID" FROM "Task" AS task, "Checkpoint" AS checkpoint, "Project" AS project 
                WHERE task."CheckpointID"=checkpoint."ID" AND checkpoint."ProjectID"=project."ID"
                 
                 */
                string sqlQuery = "SELECT project.\"ID\" FROM \"Task\" AS task, \"Checkpoint\" AS checkpoint, \"Project\" AS project WHERE task.\"CheckpointID\"=checkpoint.\"ID\" AND checkpoint.\"ProjectID\"=project.\"ID\" AND task.\"ID\"=@taskID";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@taskID", taskID);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }


        // returns all expenses for given project
        public static DataTable Expenses(int ProjectID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Expense\" WHERE \"ProjectID\"=@ProjectID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }
        // returns all expenses
        public static DataTable Expenses()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Expense\"";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        //returns all info from Department
        public static DataTable DepartmentID_data()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Department\" WHERE \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@deleted", false);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }
        public static DataTable Timesheet(int entryID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Timesheet\" WHERE \"EntryID\"=@EntryID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@EntryID", entryID);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }
        public static DataTable Timesheets(int WorkerID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Timesheet\" WHERE \"WorkerID\"=@WorkerID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@WorkerID", WorkerID);
                    command.Parameters.AddWithValue("@deleted", false);


                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };

        }
        public static DataTable Employees_id(int ID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Employee\" WHERE \"ID\"=@EmployeeID";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@EmployeeID", ID);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

		public static DataTable login(string Username, string Password, string Role)
		{
			using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
			{
				DataTable datatable = new DataTable();
				string sqlQuery = "SELECT * FROM \"Authentication\" WHERE \"Username\"=@Username  AND \"Password\"=@Password AND \"Role\"=@Role;";
				conn.Open();
				NpgsqlCommand command = new NpgsqlCommand("", conn);
				NpgsqlTransaction sqlTransaction;
				sqlTransaction = conn.BeginTransaction();
				command.Transaction = sqlTransaction;

				try
				{
					command.CommandText = sqlQuery.ToString();
					command.Parameters.Clear();
					command.Parameters.AddWithValue("@Username", Username);
					command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", Role);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
					sqlDataAdapter.Fill(datatable);

				}
				finally
				{
					conn.Close();
				}
				return datatable;
			};
		}

        public static DataTable getTasksFromDepartment(int DepartmentID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = $"SELECT T.* FROM \"Task\" AS T, \"Checkpoint\" AS C, \"Project\" AS P, \"Department\" AS D WHERE D.\"ID\"=@DepartmentID AND T.\"deleted\"=@deleted AND C.\"ID\" =T.\"CheckpointID\" AND C.\"ProjectID\"=P.\"ID\" AND P.\"DepartmentID\" = D.\"ID\";";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                    command.Parameters.AddWithValue("@deleted", false);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        public static DataTable Timesheets()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Timesheet\" WHERE \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@deleted", false);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }

        public static DataTable TimesheetEntries(int projectID)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                DataTable datatable = new DataTable();
                string sqlQuery = "SELECT * FROM \"Timesheet\" WHERE \"ProjectID\"=@ProjectID AND \"deleted\"=@deleted";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("", conn);
                NpgsqlTransaction sqlTransaction;
                sqlTransaction = conn.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    command.CommandText = sqlQuery.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectID", projectID);
                    command.Parameters.AddWithValue("@deleted", false);

                    NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(command);
                    sqlDataAdapter.Fill(datatable);

                }
                finally
                {
                    conn.Close();
                }
                return datatable;
            };
        }
    }
}