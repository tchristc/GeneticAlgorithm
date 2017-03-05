using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace Factoring
{
    public struct FactorExpon
    {
        public BigInteger factor;
        public long expon;

        public FactorExpon(BigInteger factor, long expon)
        {
            this.factor = factor;
            this.expon = expon;
        }
    }

    class QuadraticSieve
    {
        private sbyte[,] e = null;
        private sbyte[,] v = null;
        private int t, t1;
        private long B0;
        private BackgroundWorker bw;
        private HCSRAlgorithm hcr;
        private LinearAlgebra la;
        private SievePrimes sp;
        private Stopwatch sw0, sw1;
        private List<long> primes;

        public QuadraticSieve(
            int seed, int t, long B0, BackgroundWorker bw)
        {
            t1 = t + 1;
            e = new sbyte[t1, t];
            v = new sbyte[t1, t];
            this.t = t;
            this.B0 = B0;
            this.bw = bw;
            hcr = new HCSRAlgorithm(seed);
            la = new LinearAlgebra();
            sp = new SievePrimes();
            sp.Sieve(B0, ref primes);
            sw0 = new Stopwatch();
            sw1 = new Stopwatch();
        }

        public struct PrimeExpon
        {
            public long prime;
            public long expon;

            public PrimeExpon(long prime, long expon)
            {
                this.prime = prime;
                this.expon = expon;
            }
        }

        private BigInteger Sqrt(BigInteger n)
        {
            if (n == 0)
                return 0;

            BigInteger r = Sqrt(n >> 2);
            BigInteger r2 = r << 1;
            BigInteger s = r2 + 1;

            if (n < s * s)
                return r2;

            else
                return s;
        }

        private bool TrialDivision(
            BigInteger b, List<long> p, List<PrimeExpon> lpe)
        {
            BigInteger tb = b;
            PrimeExpon pe = new PrimeExpon();

            if (b == 0 || b == 1 || b == -1)
                return false;

            if (tb < 0)
            {
                tb = -tb;
                pe.prime = -1;
                pe.expon = +1;
                lpe.Add(pe);
            }

            for (int i = 1; i < p.Count; i++)
            {
                long exp = 0;
                BigInteger q = p[i];

                if (tb % q == 0)
                {
                    do
                    {
                        exp++;
                        tb /= q;
                    } while (tb % q == 0);

                    pe.prime = (long)q;
                    pe.expon = exp;
                    lpe.Add(pe);

                    if (tb == 1 || !hcr.Composite(tb, 20))
                        return true;
                }
            }

            return false;
        }

        private bool CompositeFactor(
            BigInteger factor, ref BigInteger n, ref List<FactorExpon> lfe)
        {
            FactorExpon fe = new FactorExpon();

            if (!hcr.Composite(factor, 20))
            {
                n /= factor;
                fe.factor = factor;
                fe.expon = 1;
                lfe.Add(fe);

                if (n == 1)
                    return true;

                if (!hcr.Composite(n, 20))
                {
                    fe.factor = n;
                    fe.expon = 1;
                    lfe.Add(fe);
                    n = 1;
                    return true;
                }

                return false;
            }

            // factor is composite try trial division on factor 

            BigInteger root = Sqrt(factor);

            for (int i = 0; i < primes.Count; i++)
            {
                long p = primes[i];

                if (p > root)
                    return false;

                if (factor % p == 0)
                {
                    long exp = 0;

                    do
                    {
                        exp++;
                        factor /= p;
                        n /= p;
                    } while (n % p == 0);

                    fe.factor = p;
                    fe.expon = exp;
                    lfe.Add(fe);

                    if (n == 1)
                        return true;

                    if (!hcr.Composite(n, 20))
                    {
                        fe.factor = n;
                        fe.expon = 1;
                        lfe.Add(fe);
                        n = 1;
                        return true;
                    }

                    if (factor == 1)
                        return false;
                }
            }

            return false;
        }

        public int Sieve(int maxI, int maxKernels,
            ref BigInteger n, ref List<long> times, ref List<FactorExpon> lfe)
        {
            if (n <= 1)
                return -3;

            if (!hcr.Composite(n, 20))
                return 0;

            bool done = false;
            int i = 1, count = 0, kernels = 0, result = -1;
            List<long> p = null;
            List<BigInteger> ai = new List<BigInteger>();
            List<BigInteger> bi = new List<BigInteger>();
            List<BigInteger> ff = new List<BigInteger>();

            sw0.Start();

            while (!done)
            {
                if (bw.CancellationPending)
                    return -2;

                p = new List<long>();
                p.Add(-1);

                for (int j = 0; !done && j < primes.Count; j++)
                {
                    BigInteger q = primes[j];

                    if (hcr.Jacobi(n, q) != -1)
                    {
                        p.Add((long)q);
                        done = p.Count == t;
                    }
                }

                if (!done)
                {
                    B0 += 1000000;
                    primes = new List<long>();
                    sp.Sieve(B0, ref primes);
                }
            }

            sw0.Stop();

            long millis0 = sw0.ElapsedMilliseconds, millis1 = 0;
            BigInteger m = Sqrt(n), x = 0;

            times.Add(millis0);
            sw0.Restart();

            while (i <= maxI && kernels < maxKernels)
            {
                if (bw.CancellationPending)
                    return -2;

                if (i == 1)
                {
                    x = 0;
                }

                else if (i % 2 == 0)
                {
                    x = +i / 2;
                }

                else
                {
                    x = -i / 2;
                }

                BigInteger xm = x + m, b = xm * xm - n;
                List<PrimeExpon> lpe = new List<PrimeExpon>();

                if (TrialDivision(b, p, lpe))
                {
                    for (int j = 0; j < t; j++)
                        e[count, j] = v[count, j] = 0;

                    for (int j = 0; j < t; j++)
                    {
                        long q = p[j];

                        for (int k = 0; k < lpe.Count; k++)
                        {
                            PrimeExpon pe = lpe[k];

                            if (q == pe.prime)
                            {
                                e[count, j] = (sbyte)pe.expon;
                                v[count, j] = (sbyte)(pe.expon % 2);
                                break;
                            }
                        }
                    }

                    ai.Add(xm);
                    bi.Add(b);

                    count++;

                    if (count == t1)
                    {
                        int r = 0;
                        int[] d = new int[t];

                        sw1.Restart();
                        la.KernelOverZ2(t1, t, d, ref v, ref r);
                        sw1.Stop();
                        millis1 += sw1.ElapsedMilliseconds;
                        kernels++;

                        BigInteger X = 1;

                        for (int k = 0; k < t1; k++)
                        {
                            X = (ai[k] * X) % n;
                        }

                        for (int k = 0; k < t; k++)
                        {
                            if (bw.CancellationPending)
                                return -2;

                            int dk = d[k];

                            if (dk >= 0)
                            {
                                BigInteger Y = 1;

                                for (int j = 0; j < t; j++)
                                {
                                    if (e[dk, j] % 2 != 0)
                                    {
                                        Y = (p[j] * Y) % n;
                                    }
                                }

                                BigInteger XmY = (X - Y) % n;

                                if (XmY != 0 && XmY != 1)
                                {
                                    BigInteger factor = BigInteger.GreatestCommonDivisor(XmY, n);

                                    if (factor > 1 && !ff.Contains(factor))
                                    {
                                        ff.Add(factor);

                                        if (CompositeFactor(factor, ref n, ref lfe))
                                            result = 2;

                                        else
                                            result = 1;
                                    }
                                }
                            }
                        }

                        count -= 10;

                        if (count < 0)
                            count = 0;

                        kernels++;
                    }
                }

                if (result != -1)
                    break;

                i++;
            }

            sw0.Stop();
            millis0 = sw0.ElapsedMilliseconds;
            times.Add(millis0);
            times.Add(millis1);
            return result;
        }
    }
}
