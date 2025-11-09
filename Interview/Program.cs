using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

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

        static void DisposeResource()
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - === Using IDisposable (deterministic) ===");
            using (var r1 = new Resource("R1"))
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Doing work with R1...");
                Thread.Sleep(500); // simulate work
            } // Dispose automatically called here

            Console.WriteLine($"\n{DateTime.Now:HH:mm:ss.fff} - === Not using Dispose (finalizer only) ===");
            var r2 = new Resource("R2");
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Doing work with R2...");
            Thread.Sleep(500); // simulate work
            r2 = null; // object eligible for GC

            // Force garbage collection for demo purposes
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Program finished.");
        }

        static void TestDelegates()
        {
            Console.WriteLine("=== Custom Delegate ===");
            Delegates.Notify notifier = Delegates.EmailNotification;
            notifier("Hello using custom delegate!");
            notifier += Delegates.SmsNotification;
            notifier("Hello from multicast custom delegate!");

            Console.WriteLine("\n=== Action Delegate ===");
            // Action: void return, can take parameters
            Action<string> actionNotifier = msg => Console.WriteLine($"Action: {msg}");
            actionNotifier("Hello using Action delegate!");

            Console.WriteLine("\n=== Func Delegate ===");
            // Func: returns a value, last generic type is return type
            Func<int, int, int> addFunc = (a, b) => a + b;
            int sum = addFunc(5, 7);
            Console.WriteLine($"Func: 5 + 7 = {sum}");

            Console.WriteLine("\n=== Predicate Delegate ===");
            // Predicate: takes input, returns bool
            Predicate<int> isEven = num => num % 2 == 0;
            Console.WriteLine($"Predicate: Is 10 even? {isEven(10)}");

            Console.WriteLine("\n=== Delegate as Parameter ===");
            Delegates.SendNotification("Testing delegate as parameter", Delegates.EmailNotification);
            Delegates.SendNotification("Another test", msg => Console.WriteLine($"Lambda as delegate: {msg}"));
        }

        static async void CallWithOrWithoutSyncronizationContext()
        {
            Console.WriteLine($"Main thread ID: {Thread.CurrentThread.ManagedThreadId}");
            await SynchronizationContextExample.ExampleWithoutSynchronizationContext();
            await SynchronizationContextExample.ExampleWithCustomContext();
        }

        static void GetInnerJoinResult()
        {
            var result = LinqQueries.GetInnerJoinResult();
            foreach(var resultRow in result)
            {
                Console.WriteLine($"Value of EmployeeId:-{resultRow.EmployeeId}, EmployeeName:-{resultRow.Name}, Department Location:{resultRow.Location}");
            }
        }

        public static void Main(string[] args)
        {
            //MethodHidingAndOverloading();
            //TestBasicConcepts();
            //PrintNumbersUsingManualThreads();
            //LockThreadSychronizationTechnique();
            //MonitorThreadSychronizationTechnique();

            //PrintWithoutDuplicateItems();
            //DifferenceBetweenFirstOrDefaultAndFirst();
            //DisposeResource();
            //TestDelegates();
            CallWithOrWithoutSyncronizationContext();
            GetInnerJoinResult();
        }
    }
}
