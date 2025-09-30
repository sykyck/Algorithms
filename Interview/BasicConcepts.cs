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
