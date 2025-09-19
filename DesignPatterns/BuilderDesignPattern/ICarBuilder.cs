using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.BuilderDesignPattern
{
    public interface ICarBuilder
    {
        void BuildEngine();
        void BuildWheels();
        void PaintCar();
        Car GetCar();
    }
}
