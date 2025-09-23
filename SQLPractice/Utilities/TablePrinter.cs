using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLPractice.Utilities
{
    public static class TablePrinter
    {
        public static void PrintTable<T>(IEnumerable<T> items)
        {
            if (items == null || !items.Any())
            {
                Console.WriteLine("⚠️ No data found.");
                return;
            }

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (typeof(T) == typeof(object) || props.Length == 0)
            {
                // handle anonymous type or dynamic
                foreach (var item in items)
                {
                    var dict = item.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(item, null));
                    props = dict.Keys.Select(k => item.GetType().GetProperty(k)).ToArray();
                    break;
                }
            }

            // headers
            var headers = props.Select(p => p.Name).ToArray();
            var colWidths = headers.Select(h => h.Length).ToArray();

            var rows = new List<string[]>();
            foreach (var item in items)
            {
                var values = props.Select((p, i) =>
                {
                    var val = p.GetValue(item, null);
                    var str = val?.ToString() ?? "";
                    if (str.Length > colWidths[i]) colWidths[i] = str.Length;
                    return str;
                }).ToArray();

                rows.Add(values);
            }

            // print header
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write(headers[i].PadRight(colWidths[i] + 2));
            }
            Console.WriteLine();

            // print underline
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write(new string('-', colWidths[i]) + "  ");
            }
            Console.WriteLine();

            // print rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write(row[i].PadRight(colWidths[i] + 2));
                }
                Console.WriteLine();
            }
        }
    }
}
