using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.BuilderDesignPattern
{
    public class Director
    {
        public Car Construct(ICarBuilder builder)
        {
            builder.BuildEngine();
            builder.BuildWheels();
            builder.PaintCar();
            return builder.GetCar();
        }
    }
}
