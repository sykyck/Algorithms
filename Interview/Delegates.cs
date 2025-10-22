using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    public static class Delegates
    {
        // 1. Custom delegate
        public delegate void Notify(string message);

        public static void EmailNotification(string message)
        {
            Console.WriteLine($"Email: {message}");
        }

        public static void SmsNotification(string message)
        {
            Console.WriteLine($"SMS: {message}");
        }

        // Method that accepts a custom delegate
        public static void SendNotification(string msg, Notify notifier)
        {
            notifier(msg);
        }

    }
}
