using SQLPractice.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SQLPractice.DAL
{
    public class CommonDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public CommonDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = this._configuration.GetConnectionString("Default");
        }

        public (int ScholarshipCount, int StudentCount, int DepartmentCount, int StudentScholarshipCount) GetAllTableRowsCount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string scriptPath = Path.Combine("SQL", "StoredProcedures", "GetAllTableRowsCount.sql");
                    string script = File.ReadAllText(scriptPath);
                    SqlCommand cmd = new SqlCommand(script, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            int scholarshipCount = rdr.GetInt32(rdr.GetOrdinal("ScholarshipCount"));
                            int studentCount = rdr.GetInt32(rdr.GetOrdinal("StudentCount"));
                            int departmentCount = rdr.GetInt32(rdr.GetOrdinal("DepartmentCount"));
                            int studentScholarshipCount = rdr.GetInt32(rdr.GetOrdinal("StudentScholarshipsCount"));

                            return (scholarshipCount, studentCount, departmentCount, studentScholarshipCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return zeros if no row returned
            return (0, 0, 0, 0);
        }
    }
}

