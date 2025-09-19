using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AbstractFactoryDesignPattern
{
    // Product A
    public interface IButton
    {
        void Render();
    }

    // Product B
    public interface ICheckbox
    {
        void Render();
    }
}
