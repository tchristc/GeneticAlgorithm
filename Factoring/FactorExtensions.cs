using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Factoring
{
    public static class FactorExtensions
    {
        public static List<int> Factor(this int number)
        {
            List<int> factors = new List<int>();
            int max = (int)Math.Sqrt(number);  //round down
            for (int factor = 1; factor <= max; ++factor)
            { //test from 1 to the square root, or the int below it, inclusive.
                if (number % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != number / factor)
                    { // Don't add the square root twice!  Thanks Jon
                        factors.Add(number / factor);
                    }
                }
            }
            return factors;
        }

        //public static List<BigInteger> Factor(this BigInteger number)
        //{
        //    List<BigInteger> factors = new List<BigInteger>();
        //    var max = number.Sqrt();  //round down
        //    for (BigInteger factor = 1; factor <= max; ++factor)
        //    { //test from 1 to the square root, or the int below it, inclusive.
        //        if (number % factor == 0)
        //        {
        //            factors.Add(factor);
        //            if (factor != number / factor)
        //            { // Don't add the square root twice!  Thanks Jon
        //                factors.Add(number / factor);
        //            }
        //        }
        //    }
        //    return factors;
        //}

        public static List<BigInteger> Factor(this BigInteger number)
        {
            BigInteger RANGE_SIZE = 100000000;
            //BigInteger RANGE_SIZE = 100;
            var factors = new List<BigInteger>();
            var max = number.Sqrt();
            BigInteger pos = 1;
            var tasks = new List<Task<List<BigInteger>>>();
            while (pos < max)
            {
                var start = pos;
                var end = pos + RANGE_SIZE < max ? pos + RANGE_SIZE : max;
                var task = Task.Factory.StartNew(()=> Factor(number, start, end));
                tasks.Add(task);
                pos += RANGE_SIZE+1;
            }
            var results = Task.WhenAll(tasks.ToArray()).Result.ToList();
            results.ForEach(factors.AddRange);

            return factors;
        }

        private static List<BigInteger> Factor(this BigInteger number, BigInteger start, BigInteger end)
        {
            var factors = new List<BigInteger>();
            for (var factor = start; factor <= end; ++factor)
            {
                if (number % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != number / factor)
                    {
                        factors.Add(number / factor);
                    }
                }
            }
            return factors;
        }

        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!n.IsSqrt(root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean IsSqrt(this BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
