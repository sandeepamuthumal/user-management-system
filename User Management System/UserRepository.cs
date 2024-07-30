using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace User_Management_System
{
    internal class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddUser(string userName, string userAddress, string userEmail)
        {
            const string query = "INSERT INTO users (Name,Address,Email) VALUES (@userName,@userAddress,@userEmail)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.Parameters.AddWithValue("userAddress", userAddress);
                    command.Parameters.AddWithValue("userEmail", userEmail);
                    return command.ExecuteNonQuery();
                }

            }
        }

        public int UpdateUser(string userId, string userName, string userAddress, string userEmail)
        {
            const string query = "UPDATE users SET Name = @userName , Address = @userAddress , Email = @userEmail WHERE Id = @userId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("userName", userName);
                    command.Parameters.AddWithValue("userAddress", userAddress);
                    command.Parameters.AddWithValue("userEmail", userEmail);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteUser(string userId) 
        {
            const string query = "DELETE FROM users WHERE Id = @userId";

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("userId",userId);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetUsers()
        {
            const string query = "SELECT * FROM users";
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable SearchUsers(string search)
        {
            string query = $"SELECT * FROM users WHERE Name LIKE '%{search}%'";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using( SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt; 
                }
            }
        }
    }
}
