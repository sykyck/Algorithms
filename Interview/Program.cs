using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
            basicConcepts.testRefOutAndInParameters();
        }

        static void PrintNumbersUsingManualThreads()
        {
            // Create a new thread
            Thread t1 = new Thread(ManualMultithreading.PrintNumbersUsingManualThreads);
            t1.Name = "ManualThread-t1";
            Thread t2 = new Thread(ManualMultithreading.PrintNumbersUsingManualThreads);
            t2.Name = "ManualThread-t2";

            // Start threads
            t1.Start();
            t2.Start();

            t1.Join(); //blocks the current thread(main thread) until t1 completes execution
            t2.Join(); //blocks the current thread(main thread) until t2 completes execution

            Console.WriteLine("Manual Main thread completed!");
        }

        static void LockThreadSychronizationTechnique()
        {
            Thread t1 = new Thread(ThreadSynchronizationTechniques.ImplementLockTechnique);
            t1.Name = "Lock-t1";
            Thread t2 = new Thread(ThreadSynchronizationTechniques.ImplementLockTechnique);
            t2.Name = "Lock-t2";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Lock Main Thread Ended");
        }

        static void MonitorThreadSychronizationTechnique()
        {
            Thread t1 = new Thread(ThreadSynchronizationTechniques.ImplementMonitorTechnique);
            t1.Name = "Monitor-t1";
            Thread t2 = new Thread(ThreadSynchronizationTechniques.ImplementMonitorTechnique);
            t2.Name = "Monitor-t2";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Monitor Main Thread Ended");
        }

        static void PrintWithoutDuplicateItems()
        {
            IEnumerable<string> result = LinqQueries.GetDataSourceWithoutDuplicates();
            foreach(string x in result)
            {
                Console.WriteLine($"Group key {x}");
            }
        }

        static void DifferenceBetweenFirstOrDefaultAndFirst()
        {
            LinqQueries.DifferenceBetweenFirstOrDefaultAndFirst();
        }

        public static void Main(string[] args)
        {
            //MethodHidingAndOverloading();
            //TestBasicConcepts();
            //PrintNumbersUsingManualThreads();
            //LockThreadSychronizationTechnique();
            //MonitorThreadSychronizationTechnique();

            PrintWithoutDuplicateItems();
            DifferenceBetweenFirstOrDefaultAndFirst();
        }
    }
}
