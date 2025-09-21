using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.PrototypeDesignPattern
{
    public class Address
    {
        public string City { get; set; }
    }
    public class Employee : ICloneable
    {
        public string Name { get; set; }
        public string Department { get; set; }

        public Address Address { get; set; }

        // Implement Clone() from ICloneable
        public object Clone()
        {
            return this.MemberwiseClone();  // shallow copy
        }

        public override string ToString()
        {
            return $"Employee: {Name}, Department: {Department} lives in {Address.City}";
        }
    }
}
