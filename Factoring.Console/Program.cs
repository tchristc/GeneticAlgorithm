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
                if (BigInteger.TryParse(input, out num))
                {
                    System.Console.WriteLine("The factors are: " + string.Join(",", num.Factor().OrderBy(x => x).Select(x => x.ToString())));
                }
            } while (!string.IsNullOrEmpty(input) || input.ToLower() == "q");
        }
    }
}
