using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class SynchronizationContextExample
    {

        public static async Task ExampleWithoutSynchronizationContext()
        {
            Console.WriteLine("\n--- Without SynchronizationContext ---");

            // In a console app, SynchronizationContext is null by default
            Console.WriteLine($"Before await: {Thread.CurrentThread.ManagedThreadId}, Context: {SynchronizationContext.Current?.ToString() ?? "null"}");

            await Task.Delay(1000);

            // Might resume on a *different* thread
            Console.WriteLine($"After await: {Thread.CurrentThread.ManagedThreadId}, Context: {SynchronizationContext.Current?.ToString() ?? "null"}");
        }

        public static async Task ExampleWithCustomContext()
        {
            Console.WriteLine("\n--- With Custom SynchronizationContext ---");

            var customContext = new LoggingSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(customContext);

            Console.WriteLine($"Before await: {Thread.CurrentThread.ManagedThreadId}, Context: {SynchronizationContext.Current.GetType().Name}");

            await Task.Delay(1000);

            // Resumes using our custom context
            Console.WriteLine($"After await: {Thread.CurrentThread.ManagedThreadId}, Context: {SynchronizationContext.Current.GetType().Name}");
        }
    }

    class LoggingSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Console.WriteLine($"[Custom Context] Posting work on Thread {Thread.CurrentThread.ManagedThreadId}");
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine($"[Custom Context] Executing on Thread {Thread.CurrentThread.ManagedThreadId}");
                SetSynchronizationContext(this);
                d(state);
            });
        }
    }

}
