using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    public class BasicConcepts
    {
        class Person
        {
            public string Name { get; set; }
        }

        private void AddValueToReferenceVariable(ref int number, int value)
        {
            number += 5;
        }

        private void DivideAndReturnResultAsOutputVariable(int a, int b, out int result)
        {
            if (b == 0)
            {
                result = 0;
            }
            result = a / b;
        }

        private void PrintPassedValue(in int number)
        {
            Console.WriteLine($"Variable passed to PrintPassedValue={number}");
            //number = 20; ❌ Not allowed (read-only)
        }

        public void testRefOutAndInParameters()
        {
            // Passes argument by reference.
            // Caller must initialize the variable before passing.
            // Callee (method) can read and modify it.
            int x = 10;
            AddValueToReferenceVariable(ref x, 5); // x becomes 15
            if(x==15)
            {
                Console.WriteLine($"Variable passed as reference value of x={x} and value added={5}");
            }

            //out also passes argument by reference.
            // Caller does not need to initialize before passing.
            // (method) must assign a value before returning.
            int value;
            DivideAndReturnResultAsOutputVariable(10, 2, out value); // value = 5
            Console.WriteLine($"Variable passed as out value of it={value}");

            int y = 50;
            PrintPassedValue(in y); // Prints 50

        }


        public void valueTypeAndReferenceType()
        {
            int a = 10;
            int b = a;   // b gets a copy of a
            b = 20;

            if(a==10)
            {
                Console.WriteLine($"int is value type value of a={a} and b={b}");
            }

            Person p1 = new Person { Name = "Alice" };
            Person p2 = p1;  // p2 references the same object as p1

            p2.Name = "Bob";

            if(p1.Name == "Bob")
            {
                Console.WriteLine($"Class is reference type value of p1.Name={p1.Name} and p2.Name={p2.Name}");
            }


        }
    }
}
