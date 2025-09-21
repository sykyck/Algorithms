using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.PrototypeDesignPattern
{
    public enum CopyType
    {
        Shallow,
        Deep
    }

    public class Address : ICloneable
    {
        public string City { get; set; }

        public CopyType CopyType { get; set; }

        // Deep clone for Address
        public object Clone()
        {
            if (CopyType == CopyType.Shallow)
            {
                // Just copy reference (shallow)
                return this.MemberwiseClone();
            }
            else
            {
                // Deep copy → new object
                return new Address { City = this.City, CopyType = this.CopyType };
            }
        }
    }
    public class Employee : ICloneable
    {
        public string Name { get; set; }
        public string Department { get; set; }

        public CopyType CopyType { get; set; }   // decides shallow vs deep

        public Address Address { get; set; }

        // Implement Clone() from ICloneable
        public object Clone()
        {
            if (CopyType == CopyType.Shallow)
            {
                // Shallow copy → MemberwiseClone()
                return this.MemberwiseClone();
            }
            else
            {
                // Deep copy → also clone nested objects
                return new Employee
                {
                    Name = this.Name,
                    CopyType = this.CopyType,  // keep same mode
                    Address = (Address)this.Address.Clone() // delegate to Address.Clone()
                };
            }
        }

        public override string ToString()
        {
            return $"Employee: {Name}, Department: {Department} lives in {Address.City}";
        }
    }
}
