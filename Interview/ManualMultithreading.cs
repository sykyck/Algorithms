using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Interview
{
    public class ManualMultithreading
    {
        public static void PrintNumbersUsingManualThreads()
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Number: {i} (Thread Name: {Thread.CurrentThread.Name})");
                Thread.Sleep(500); // simulate work
            }
        }
    }
}
