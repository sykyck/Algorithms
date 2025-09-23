using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using SQLPractice.DAL;
using SQLPractice.Models;
using System.Collections.Generic;

namespace SQLPractice
{
    class Program
    {
        private static IConfiguration _iconfiguration;
        static void Main(string[] args)
        {
            GetAppSettingsFile();
            var students = GetStudentsData();
            if(students.Count != 5)
            {
               SeedStudentData();
            }
            var departments = GetDepartments();
            if(departments.Count != 5)
            {
               SeedDepartmentData();
            }
            //ShowAdventureDepartments();
            Console.WriteLine("Press any key to stop.");
            Console.ReadKey();
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }    
        
        static void SeedStudentData()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            var rowsAffected = studentDAL.SeedStudents();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }

        static IList<Student> GetStudentsData()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            return studentDAL.GetAllStudents();
        }

        static void SeedDepartmentData()
        {
            var deptDAL = new DepartmentDAL(_iconfiguration);
            var rowsAffected = deptDAL.SeedDepartments();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }
        static IList<Department> GetDepartments()
        {
            var deptDAL = new DepartmentDAL(_iconfiguration);
            return deptDAL.GetAllDepartments();           
        }
    }
}
