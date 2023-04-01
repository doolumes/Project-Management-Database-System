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
                string sqlQuery = "SELECT * FROM \"Expense\" WHERE \"ProjectID\"=@ProjectID";
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

        //returns all info from Employees using ID
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
    }
}