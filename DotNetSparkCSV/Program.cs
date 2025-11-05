using System;
using Microsoft.Spark.Sql;

namespace DotNetSparkCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 Starting .NET Spark Application...");

            // Display environment information
            Console.WriteLine($"Environment:");
            Console.WriteLine($"  DOTNET_WORKER_DIR: {Environment.GetEnvironmentVariable("DOTNET_WORKER_DIR")}");
            Console.WriteLine($"  DOTNETBACKEND_PORT: {Environment.GetEnvironmentVariable("DOTNETBACKEND_PORT")}");
            Console.WriteLine($"  SPARK_MASTER: {Environment.GetEnvironmentVariable("SPARK_MASTER")}");

            try
            {
                Console.WriteLine("Creating Spark session...");

                // Create Spark session with explicit configuration for Docker
                var spark = SparkSession
                    .Builder()
                    .AppName("DotNet Spark CSV Processing")
                    .Config("spark.master", Environment.GetEnvironmentVariable("SPARK_MASTER") ?? "local")
                    .Config("spark.driver.host", "spark-dotnet")
                    .Config("spark.driver.bindAddress", "0.0.0.0")
                    .Config("spark.sql.adaptive.enabled", "true")
                    .Config("spark.sql.adaptive.coalescePartitions.enabled", "true")
                    // Critical: Configure Spark to use .NET worker
                    .Config("spark.driver.extraJavaOptions",
                        $"-Ddotnet.driver.port={Environment.GetEnvironmentVariable("DOTNETBACKEND_PORT")} " +
                        $"-Ddotnet.driver.host=spark-dotnet")
                    .Config("spark.executor.extraJavaOptions",
                        $"-Ddotnet.driver.port={Environment.GetEnvironmentVariable("DOTNETBACKEND_PORT")} " +
                        $"-Ddotnet.driver.host=spark-dotnet")
                    .GetOrCreate();

                // Set log level to see more details
                spark.SparkContext.SetLogLevel("INFO");

                Console.WriteLine("✅ Spark session created successfully!");

                // Test with simple data first
                Console.WriteLine("Testing Spark connection with simple data...");
                var testData = spark.Sql("SELECT 'Hello Spark from Docker!' as greeting, 42 as number");
                testData.Show();

                // Process CSV data if available
                ProcessCSVData(spark);

                spark.Stop();
                Console.WriteLine("✅ Application completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                Environment.Exit(1);
            }
        }

        static void ProcessCSVData(SparkSession spark)
        {
            try
            {
                Console.WriteLine("Reading CSV data...");

                string dataPath = "/app/data";

                // Check if data directory exists
                if (!System.IO.Directory.Exists(dataPath))
                {
                    Console.WriteLine($"❌ Data directory not found: {dataPath}");
                    return;
                }

                var csvFiles = System.IO.Directory.GetFiles(dataPath, "*.csv");
                if (csvFiles.Length == 0)
                {
                    Console.WriteLine("❌ No CSV files found in data directory");
                    return;
                }

                Console.WriteLine($"Found {csvFiles.Length} CSV file(s):");
                foreach (var file in csvFiles)
                {
                    Console.WriteLine($"  - {System.IO.Path.GetFileName(file)}");
                }

                // Read and process CSV files
                var df = spark.Read()
                    .Option("header", "true")
                    .Option("inferSchema", "true")
                    .Csv($"{dataPath}/*.csv");

                // Show results
                Console.WriteLine("DataFrame schema:");
                df.PrintSchema();

                Console.WriteLine("First 10 rows:");
                df.Show(10);

                var count = df.Count();
                Console.WriteLine($"Total records: {count}");

                // Perform a simple operation
                var columnNames = df.Columns();
                if (columnNames.Count > 0)
                {
                    var firstColumn = columnNames[0];
                    Console.WriteLine($"Showing distinct values for column: {firstColumn}");
                    df.Select(firstColumn).Distinct().Show();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CSV: {ex.Message}");
            }
        }
    }
}