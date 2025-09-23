using SQLPractice.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace SQLPractice.DAL
{
    public class StudentDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public StudentDAL(IConfiguration configuration)
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

        public List<Student> GetAllStudents()
        {
            var lstStudents = new List<Student>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "GetAllStudents.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        lstStudents.Add(new Student
                        {
                            StudentId = rdr.GetInt32("StudentId"),
                            SemesterFees = rdr.GetInt32("SemesterFees"),
                            DepartmentId = rdr.GetInt32("DepartmentId"),
                            FirstName = rdr.GetString("FirstName"),
                            LastName = rdr.GetString("LastName"),
                            DateOfBirth = rdr.GetDateTime("DateOfBirth"),
                            Email = rdr.GetString("Email"),
                            IsActive = rdr.GetBoolean("IsActive"),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstStudents;
        }
    }
}

