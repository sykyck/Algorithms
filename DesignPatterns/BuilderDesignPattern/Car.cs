using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.BuilderDesignPattern
{
    public class Car
    {
        public string Engine { get; set; }
        public string Wheels { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"Car with {Engine}, {Wheels} wheels, and {Color} color.";
        }
    }
}
