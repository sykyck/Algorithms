using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AbstractFactoryDesignPattern
{
    // Windows Variants
    public class WindowsButton : IButton
    {
        public void Render() => Console.WriteLine("Rendering a Windows Button");
    }

    public class WindowsCheckbox : ICheckbox
    {
        public void Render() => Console.WriteLine("Rendering a Windows Checkbox");
    }

    // Mac Variants
    public class MacButton : IButton
    {
        public void Render() => Console.WriteLine("Rendering a Mac Button");
    }

    public class MacCheckbox : ICheckbox
    {
        public void Render() => Console.WriteLine("Rendering a Mac Checkbox");
    }
}
