using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using SQLPractice.DAL;
using SQLPractice.Models;
using System.Collections.Generic;
using SQLPractice.Utilities;

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


            bool running = true;
            while (running)
            {
                Console.WriteLine("\n===== MENU =====");
                Console.WriteLine("1 - Show Students with Department & Fee Rank");
                Console.WriteLine("Q - Quit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadKey(intercept: true).KeyChar; // reads a single key
                Console.WriteLine(); // for spacing

                switch (input)
                {
                    case '1':
                        GetStudentsRankedBySemesterFees();
                        break;
                    case 'q':
                    case 'Q':
                        running = false;
                        break;
                    default:
                        Console.WriteLine("❌ Invalid option. Try again.");
                        break;
                }
            }

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

        static void GetStudentsRankedBySemesterFees()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            IList<dynamic> result = studentDAL.GetStudentsRankedBySemesterFees();
            TablePrinter.PrintTable(result);
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
