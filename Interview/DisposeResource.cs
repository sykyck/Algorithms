using System;
using System.Collections.Generic;
using System.Text;

namespace Interview
{
    class Resource : IDisposable
    {
        private bool disposed = false;
        private string name;

        public Resource(string name)
        {
            this.name = name;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {name}: Acquired resource");
        }

        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // prevent finalizer from running
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {name}: Dispose called - managed cleanup");
                }

                // Release unmanaged resources (simulated)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {name}: Cleanup unmanaged resources");
                disposed = true;
            }
        }

        // Finalizer (called by GC if Dispose not called)
        ~Resource()
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {name}: Finalizer called");
            Dispose(false);
        }
    }
}
