using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCollections
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<int, string> d = new Dictionary<int, string>();

            d[44] = "United Kingdom";
            d[33] = "France";
            d[31] = "Netherlands";
            d[55] = "Brazil";

            Console.WriteLine("The 33 code is for {0}", d[33]);

            foreach (KeyValuePair <int, string> p in d)
            {
                int code = p.Key;
                string country = p.Value;

                Console.WriteLine("{0} : {1}", code, country);
            }
        }
    }
}
