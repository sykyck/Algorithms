using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;

namespace Interview
{
    public class Employee
    {
        public Employee(int employeeId, string name, int departmentId, decimal salary) {
            EmployeeId = employeeId;
            Name = name;
            DepartmentId = departmentId;
            Salary = salary;
        }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }

    public class Department
    {
        public Department(int departmentId, string name, string location)
        {
            DepartmentId = departmentId;
            DepartmentName = name;
            Location = location;
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
    }

    public class LinqQueries
    {
        //data source
        private static IList<string> stringDataSource = new List<string>()
        {
            "aman","suresh","aman","rajesh","rakesh"
        };

        //data source
        private static IList<Employee> employeesDataSource = new List<Employee>()
        {
            new Employee(1,"emplyee1",1,50000),
            new Employee(2,"emplyee2",1,60000),
            new Employee(3,"emplyee3",2,40000),
            new Employee(4,"emplyee4",3,20000),
            new Employee(5,"emplyee5",3,10000),
        };

        private static IList<Department> departmentsDataSource = new List<Department>()
        {
            new Department(1,"emplyee1","delhi"),
            new Department(2,"emplyee2","hyderabd"),
            new Department(3,"emplyee3","gurgaon"),
        };

        public static IEnumerable<string> GetDataSourceWithoutDuplicates()
        {
            return stringDataSource.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key)
                           .Union(stringDataSource.GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key));
        }

        //inner join
        public static IEnumerable<dynamic> GetInnerJoinResult()
        {
            var result = employeesDataSource.Join(departmentsDataSource,
                emp => emp.DepartmentId,
                dept => dept.DepartmentId,
                (emp, dept) => new
                {
                    emp.EmployeeId,
                    emp.Name,
                    emp.Salary,
                    dept.DepartmentName,
                    dept.Location
                });

            return result;
        }

        public static void DifferenceBetweenFirstOrDefaultAndFirst()
        {
            try
            {
                // ✅ First() - Throws exception if not found
                string firstString = stringDataSource.First(n => n == "vaibhav");
                // ❌ Throws InvalidOperationException because no element is found

                Console.WriteLine($"No Exception Occurred First() Returned {firstString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred {ex.Message}");
            }

            // ✅ FirstOrDefault()
            string firstNameVaibhav = stringDataSource.FirstOrDefault(n => n == "vaibhav");
            // Returns Empty string (default for string)

            Console.WriteLine($"FirstOrDefault Returned {firstNameVaibhav}");
        }
    }
}
