using AdventureData.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace AdventureData.DAL
{
    public class StudenttDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public StudenttDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = this._configuration.GetConnectionString("Default");
        }

        public int SeedStudents()
        {
            int rowsAffected;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "SeedData", "Students.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery(); // ✅ correct for CREATE/INSERT
                    Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowsAffected;
        }
    }
}

