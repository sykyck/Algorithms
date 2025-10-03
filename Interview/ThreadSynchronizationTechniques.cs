using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Interview
{
    public class ThreadSynchronizationTechniques
    {
        static int counter = 0;
        static readonly object lockObj = new object();
        private static void Increment()
        {
            for (int i = 0; i < 5; i++)
            {
                counter++;
                Console.WriteLine($"Value of counter after increment done by {Thread.CurrentThread.Name} = {counter} ");
            }
        }
        public static void ImplementLockTechnique()
        {
            lock (lockObj) // only one thread at a time
            {
                Increment();
            }
        }

        public static void ImplementMonitorTechnique()
        {
            Console.WriteLine(Thread.CurrentThread.Name + " Trying to enter into the critical section");

            try
            {
                Monitor.Enter(lockObj);
                Console.WriteLine(Thread.CurrentThread.Name + " Entered into the critical section");
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(i + ",");
                }
                Console.WriteLine();
            }
            finally
            {
                Monitor.Exit(lockObj);
                Console.WriteLine(Thread.CurrentThread.Name + " Exit from critical section");
            }
        }


    }
}
