using DesignPatterns.AbstractFactoryDesignPattern;
using DesignPatterns.BuilderDesignPattern;
using DesignPatterns.FactoryDesignPattern;
using DesignPatterns.PrototypeDesignPattern;
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
            // Original object
            Employee emp1 = new Employee { Name = "Alice", Department = "HR" };
            Console.WriteLine("Original: " + emp1);

            // Clone object
            Employee emp2 = (Employee)emp1.Clone();
            emp2.Name = "Bob";   // change clone’s name

            Console.WriteLine("Clone: " + emp2);
            Console.WriteLine("Original after clone: " + emp1);
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
        }
    }
}
