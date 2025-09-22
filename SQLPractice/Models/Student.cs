using System;
using System.Collections.Generic;
using System.Text;

namespace SQLPractice.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public Boolean IsActive { get; set; }
    }
}
