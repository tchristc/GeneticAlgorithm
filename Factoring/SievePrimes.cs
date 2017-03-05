using System.Collections.Generic;

namespace Factoring
{
    class SievePrimes
    {
        public void Sieve(long B0, ref List<long> primes)
        {
            // Sieve of Eratosthenes
            // find all prime numbers
            // less than or equal B0

            bool[] sieve = new bool[B0 + 1];
            long c = 3, i, inc;

            sieve[2] = true;

            for (i = 3; i <= B0; i++)
                if (i % 2 == 1)
                    sieve[i] = true;

            do
            {
                i = c * c;
                inc = c + c;

                while (i <= B0)
                {
                    sieve[i] = false;

                    i += inc;
                }

                c += 2;

                while (!sieve[c])
                    c++;
            } while (c * c <= B0);

            if (primes == null)
                primes = new List<long>();

            for (i = 2; i <= B0; i++)
                if (sieve[i])
                    primes.Add(i);
        }
    }
}