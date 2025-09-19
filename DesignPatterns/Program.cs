using DesignPatterns.AbstractFactoryDesignPattern;
using DesignPatterns.FactoryDesignPattern;
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

        static void Main(string[] args)
        {
            Console.WriteLine("Factory Design Pattern Example!");
            FactoryDesignPattern();
            Console.WriteLine("!!!!!!!!!!!!!!!");
            Console.WriteLine("Abstract Factory Design Pattern Example!");
            AbstractFactoryDesignPattern();
        }
    }
}
