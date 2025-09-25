using SQLPractice.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SQLPractice.DAL
{
    public class ScholarshipDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public ScholarshipDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = this._configuration.GetConnectionString("Default");
        }

        public int SeedScholarships()
        {
            int rowsAffected;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "SeedData", "Scholarships.sql");
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

        public int SeedStudentScholarships()
        {
            int rowsAffected;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "SeedData", "StudentScholarships.sql");
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

