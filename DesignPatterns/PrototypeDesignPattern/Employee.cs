using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.PrototypeDesignPattern
{
    public class Employee : ICloneable
    {
        public string Name { get; set; }
        public string Department { get; set; }

        // Implement Clone() from ICloneable
        public object Clone()
        {
            return this.MemberwiseClone();  // shallow copy
        }

        public override string ToString()
        {
            return $"Employee: {Name}, Department: {Department}";
        }
    }
}
