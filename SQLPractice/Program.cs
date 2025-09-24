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
                Console.WriteLine("2 - Add Bulk Students Data");
                Console.WriteLine("3 - Delete Bulk Students Data");
                Console.WriteLine("4 - Get Students Data By Optimized Query");
                Console.WriteLine("5 - Get Students Data By Unoptimized Query");
                Console.WriteLine("Q - Quit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadKey(intercept: true).KeyChar; // reads a single key
                Console.WriteLine(); // for spacing

                switch (input)
                {
                    case '1':
                        GetStudentsRankedBySemesterFees();
                        break;
                    case '2':
                        AddBulkStudentsData();
                        break;
                    case '3':
                        DeleteBulkStudentsData();
                        break;
                    case '4':
                        GetStudentsDataByOptimizedQuery();
                        break;
                    case '5':
                        GetStudentsDataByUnoptimizedQuery();
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

        static void GetStudentsDataByOptimizedQuery()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            long elapsedTime = studentDAL.GetStudentsByOptimizedQuery();
            Console.WriteLine($"Elapsed Time For Optimized Query: {elapsedTime}"); ;
        }

        static void GetStudentsDataByUnoptimizedQuery()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            long elapsedTime = studentDAL.GetStudentsByUnoptimizedQuery();
            Console.WriteLine($"Elapsed Time For Unoptimized Query: {elapsedTime}"); ;
        }

        static void AddBulkStudentsData()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            int rowsAffected = studentDAL.AddBulkStudents();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }

        static void DeleteBulkStudentsData()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            int rowsAffected = studentDAL.DeleteBulkStudents();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
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
