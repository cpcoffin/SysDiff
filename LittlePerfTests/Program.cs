using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace LittlePerfTests
{
    class Program
    {
        static Regex wsRegex = new Regex(@"^\s*$");
        static bool testWs1(string input)
        {
            return input.Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "") == "";
        }

        static bool testWs2(string input)
        {
            return wsRegex.IsMatch(input);
        }

        static void Main(string[] args)
        {
            Stopwatch s1 = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                bool a = testWs1(String.Concat(Enumerable.Repeat(" ", i % 10000)));
            }
            s1.Stop();

            Stopwatch s2 = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                bool a = testWs2(String.Concat(Enumerable.Repeat(" ", i % 10000)));
            }
            s2.Stop();

            Console.WriteLine("s1 is {0} and s2 is {1}", s1.ElapsedMilliseconds, s2.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
