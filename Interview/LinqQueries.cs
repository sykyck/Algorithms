using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview
{
    public class LinqQueries
    {
        //data source
        private static IList<string> stringDataSource = new List<string>()
        {
            "aman","suresh","aman","rajesh","rakesh"
        };

        public static IEnumerable<string> GetDataSourceWithoutDuplicates()
        {
            return stringDataSource.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key)
                           .Union(stringDataSource.GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key));
        }

        public static void DifferenceBetweenFirstOrDefaultAndFirst()
        {
            try
            {
                // ✅ First() - Throws exception if not found
                string firstString = stringDataSource.First(n => n == "vaibhav");
                // ❌ Throws InvalidOperationException because no element is found

                Console.WriteLine($"No Exception Occurred First() Returned {firstString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred {ex.Message}");
            }

            // ✅ FirstOrDefault()
            string firstNameVaibhav = stringDataSource.FirstOrDefault(n => n == "vaibhav");
            // Returns Empty string (default for string)

            Console.WriteLine($"FirstOrDefault Returned {firstNameVaibhav}");
        }
    }
}
