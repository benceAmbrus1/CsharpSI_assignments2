using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;


namespace LookupCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            ListDictionary ld = new ListDictionary(new CaseInsensitiveComparer(CultureInfo.InvariantCulture));

            ld["Estados Unidos"] = "United States of America";
            ld["Canadá"] = "Canada";
            ld["España"] = "Spain";

            Console.WriteLine(ld["españa"]);
            Console.WriteLine(ld["CANADÁ"]);
            Console.Read();
        }
    }
}
