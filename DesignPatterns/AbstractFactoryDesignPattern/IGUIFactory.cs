using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AbstractFactoryDesignPattern
{
    public interface IGUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }
}
