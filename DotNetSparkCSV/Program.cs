using System;
using Microsoft.Spark.Sql;

namespace DotNetSparkCSV
{
    class Program
    {
        static void Main()
        {
            var spark = SparkSession.Builder()
                .AppName("DotNetSpark8AutoDocker")
                .Master("local[*]")
                .GetOrCreate();

            Console.WriteLine("Spark session started.");

            var df = spark.Read()
                .Option("header", true)
                .Csv("/data/sample.csv");

            df.Show();

            var adults = df.Filter(df["Age"] >= 30);
            adults.Show();

            spark.Stop();
            Console.WriteLine("Spark session stopped.");
        }
    }
}
