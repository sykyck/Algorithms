using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SingletonDesignPattern
{
    public sealed class Singleton
    {
        // Private static variable that holds the single instance
        private static readonly Lazy<Singleton> instance =
            new Lazy<Singleton>(() => new Singleton());

        // Private constructor so it cannot be instantiated from outside
        private Singleton()
        {
            Console.WriteLine("Singleton Instance Created");
        }

        // Public static property to access the instance
        public static Singleton Instance
        {
            get { return instance.Value; }
        }

        // Example method
        public void DoSomething()
        {
            Console.WriteLine("Doing something with the Singleton instance.");
        }
    }

}
