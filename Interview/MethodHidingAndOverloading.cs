using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    public class BaseClass
    {
        public void Method()
        {
            Console.WriteLine("BaseClass-Method");
        }
    }

    public class DerivedClass1 : BaseClass
    {
        public new virtual void Method()
        {
            Console.WriteLine("Derived1-Method");
        }
    }

    public class DerivedClass2 : DerivedClass1
    {
        public override void Method()
        {
            Console.WriteLine("Derived2-Method");
        }
    }

    public class MethodHidingAndOverloading
    {
        public static void Main(string[] args)
        {
            BaseClass bc = new DerivedClass1();
            bc.Method();
            DerivedClass1 dc = new DerivedClass2();
            dc.Method();
        }
    }
}
