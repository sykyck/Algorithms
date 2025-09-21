using DesignPatterns.AbstractFactoryDesignPattern;
using DesignPatterns.BuilderDesignPattern;
using DesignPatterns.FactoryDesignPattern;
using DesignPatterns.PrototypeDesignPattern;
using DesignPatterns.SingletonDesignPattern;
using System;

namespace DesignPatterns
{
    class Program
    {
        static void FactoryDesignPattern()
        {
            ShapeFactory factory = new ShapeFactory();

            IShape shape1 = factory.GetShape("circle");
            shape1.Draw();   // Output: Drawing a Circle

            IShape shape2 = factory.GetShape("rectangle");
            shape2.Draw();   // Output: Drawing a Rectangle

            IShape shape3 = factory.GetShape("square");
            shape3.Draw();   // Output: Drawing a Square
        }

        static void AbstractFactoryDesignPattern()
        {
            // Suppose we detect OS = Windows
            IGUIFactory factory = new WindowsFactory();

            IButton button = factory.CreateButton();
            ICheckbox checkbox = factory.CreateCheckbox();

            button.Render();     // Rendering a Windows Button
            checkbox.Render();   // Rendering a Windows Checkbox

            // Switch to Mac
            factory = new MacFactory();
            button = factory.CreateButton();
            checkbox = factory.CreateCheckbox();

            button.Render();     // Rendering a Mac Button
            checkbox.Render();   // Rendering a Mac Checkbox
        }

        static void BuilderDesignPattern()
        {
            Director director = new Director();

            ICarBuilder sportsBuilder = new SportsCarBuilder();
            Car sportsCar = director.Construct(sportsBuilder);
            Console.WriteLine(sportsCar);

            ICarBuilder suvBuilder = new SUVBuilder();
            Car suvCar = director.Construct(suvBuilder);
            Console.WriteLine(suvCar);
        }

        static void ProtoTypeDesignPattern()
        {
            var emp1 = new Employee
            {
                Name = "Alice",
                Address = new Address { City = "New York", CopyType = CopyType.Shallow },
                CopyType = CopyType.Shallow
            };

            // Shallow copy
            var emp2 = (Employee)emp1.Clone();
            emp2.Name = "Bob";
            emp2.Address.City = "Los Angeles"; // affects both because shallow

            Console.WriteLine("Shallow Copy Test:");
            Console.WriteLine("Original: " + emp1);
            Console.WriteLine("Shallow Copy: " + emp2);

            Console.WriteLine("Switching To Deep Copy");

            // Switch to deep copy
            emp1.CopyType = CopyType.Deep;
            emp1.Address.CopyType = CopyType.Deep;

            var emp3 = (Employee)emp1.Clone();
            emp3.Name = "Charlie";
            emp3.Address.City = "Chicago"; // independent copy

            Console.WriteLine("Deep Copy Test:");
            Console.WriteLine("Original: " + emp1);
            Console.WriteLine("Deep Copy: " + emp3);
        }

        static void SingletonDesignPattern()
        {
            var s1 = Singleton.Instance;
            s1.DoSomething();

            var s2 = Singleton.Instance;

            Console.WriteLine(ReferenceEquals(s1, s2)
                ? "Both references point to the same instance"
                : "Different instances");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Factory Design Pattern Example!");
            FactoryDesignPattern();
            Console.WriteLine("!!!!!!!!!!!!!!!");
            Console.WriteLine("Abstract Factory Design Pattern Example!");
            AbstractFactoryDesignPattern();
            Console.WriteLine("!!!!!!!!!!!!!!!");
            Console.WriteLine("Builder Design Pattern Example!");
            BuilderDesignPattern();
            Console.WriteLine("!!!!!!!!!!!!!!!");
            Console.WriteLine("Protype Design Pattern Example!");
            ProtoTypeDesignPattern();
            Console.WriteLine("!!!!!!!!!!!!!!!");
            Console.WriteLine("Singleton Design Pattern Example!");
            SingletonDesignPattern();
        }
    }
}
