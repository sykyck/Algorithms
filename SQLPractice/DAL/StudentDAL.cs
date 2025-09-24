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

        public int AddBulkStudents()
        {
            int rowsAffected;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "AddBulkStudentRecords.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery(); // ✅ correct for CREATE/INSERT
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowsAffected;
        }

        public int DeleteBulkStudents()
        {
            int rowsDeleted =0;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "DeleteBulkStudentRecords.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    var result = cmd.ExecuteScalar(); // ✅ fetches the SELECT @DeletedRows
                    if (result != null && result != DBNull.Value)
                    {
                        rowsDeleted = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowsDeleted;
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

        public List<dynamic> GetStudentsRankedBySemesterFees()
        {
            var lstStudents = new List<dynamic>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "GetStudentsRankedBySemesterFees.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        dynamic resultRow = new {
                            StudentId = rdr.GetInt32("StudentId"),
                            FirstName = rdr.GetString("FirstName"),
                            LastName = rdr.GetString("LastName"),
                            DepartmentName = rdr.GetString("DepartmentName"),
                            SemesterFees = rdr.GetInt32("SemesterFees"),
                            FeeRank = rdr.GetInt64("FeeRank"),
                            FeeDenseRank = rdr.GetInt64("FeeDenseRank"),
                            RowNumber = rdr.GetInt64("RowNumber")
                        };
                        lstStudents.Add(resultRow);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstStudents;
        }

        public List<dynamic> GetStudentsByOptimizedQuery()
        {
            var lstStudents = new List<dynamic>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "GetStudentsRankedBySemesterFees.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        dynamic resultRow = new
                        {
                            StudentId = rdr.GetInt32("StudentId"),
                            FirstName = rdr.GetString("FirstName"),
                            LastName = rdr.GetString("LastName"),
                            DepartmentName = rdr.GetString("DepartmentName"),
                            SemesterFees = rdr.GetInt32("SemesterFees"),
                            FeeRank = rdr.GetInt64("FeeRank"),
                            FeeDenseRank = rdr.GetInt64("FeeDenseRank"),
                            RowNumber = rdr.GetInt64("RowNumber")
                        };
                        lstStudents.Add(resultRow);
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

