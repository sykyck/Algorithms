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
            (int scholarShipCount, int studentCount, int departmentCount, int studentScholarshipsCount) = GetAllTableRowsCount();
            if(studentCount != 5)
            {
               SeedStudentData();
            }
            if(departmentCount != 5)
            {
               SeedDepartmentData();
            }
            if (scholarShipCount != 2)
            {
               SeedScholarshipData();
            }
            if(studentScholarshipsCount != 5)
            {
               SeedStudentScholarshipsData();
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
                Console.WriteLine("6 - Update Students Payable Fees");
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
                    case '6':
                        UpdateStudentsPayableFeesData();
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

        static (int, int, int, int) GetAllTableRowsCount()
        {
            var commonDAL = new CommonDAL(_iconfiguration);
            return commonDAL.GetAllTableRowsCount();
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

        static void UpdateStudentsPayableFeesData()
        {
            var studentDAL = new StudentDAL(_iconfiguration);
            int rowsAffected = studentDAL.UpdateStudentPayableFees();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }

        static void SeedDepartmentData()
        {
            var deptDAL = new DepartmentDAL(_iconfiguration);
            var rowsAffected = deptDAL.SeedDepartments();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }

        static void SeedScholarshipData()
        {
            var scholarshipDAL = new ScholarshipDAL(_iconfiguration);
            var rowsAffected = scholarshipDAL.SeedScholarships();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }

        static void SeedStudentScholarshipsData()
        {
            var scholarshipDAL = new ScholarshipDAL(_iconfiguration);
            var rowsAffected = scholarshipDAL.SeedStudentScholarships();
            Console.WriteLine($"✅ Script executed. Rows affected: {rowsAffected}");
        }
    }
}
