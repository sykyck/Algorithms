using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.BuilderDesignPattern
{
    public class SportsCarBuilder : ICarBuilder
    {
        private Car _car = new Car();

        public void BuildEngine() => _car.Engine = "V8 Engine";
        public void BuildWheels() => _car.Wheels = "Alloy";
        public void PaintCar() => _car.Color = "Red";

        public Car GetCar() => _car;
    }

    public class SUVBuilder : ICarBuilder
    {
        private Car _car = new Car();

        public void BuildEngine() => _car.Engine = "V6 Engine";
        public void BuildWheels() => _car.Wheels = "Steel";
        public void PaintCar() => _car.Color = "Black";

        public Car GetCar() => _car;
    }

}
