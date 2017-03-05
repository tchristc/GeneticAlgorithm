using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Factoring.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "";
            do
            {
                System.Console.WriteLine("What number do you want to factor: ");
                input = System.Console.ReadLine();
                //var num = 0;
                //if (int.TryParse(input, out num))
                //{
                //    System.Console.WriteLine("The factors are: " + string.Join(",", num.Factor().OrderBy(x=>x).Select(x => x.ToString())));
                //}
                BigInteger num = 0;
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                if (BigInteger.TryParse(input, out num))
                {
                    System.Console.WriteLine("The factors are: " + string.Join(",", num.Factor().OrderBy(x => x).Select(x => x.ToString())));
                }
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                var ts = stopWatch.Elapsed;
                var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:000}";
                System.Console.WriteLine("RunTime " + elapsedTime);
            } while (!string.IsNullOrEmpty(input) || input.ToLower() == "q");
        }
    }
}
