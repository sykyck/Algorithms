using SQLPractice.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SQLPractice.DAL
{
    public class DepartmentDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DepartmentDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = this._configuration.GetConnectionString("Default");
        }

        public int SeedDepartments()
        {
            int rowsAffected;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "SeedData", "Departments.sql");
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

        public List<Department> GetAllDepartments()
        {
            var lstDepartments = new List<Department>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Departments", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        lstDepartments.Add(new Department
                        {
                            DepartmentID = rdr.GetInt32("DepartmentID"),
                            Name = rdr.GetString("Name"),
                            ModifiedDate = rdr.GetDateTime("ModifiedDate")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDepartments;
        }
    }
}

