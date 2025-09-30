using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    public class Program
    {
        static void MethodHidingAndOverloading()
        {
            BaseClass bc = new DerivedClass1();
            bc.Method();
            DerivedClass1 dc = new DerivedClass2();
            dc.Method();
        }

        static void TestBasicConcepts()
        {
            BasicConcepts basicConcepts = new BasicConcepts();
            basicConcepts.valueTypeAndReferenceType();
        }
        public static void Main(string[] args)
        {
            MethodHidingAndOverloading();
            TestBasicConcepts();
        }
    }
}
