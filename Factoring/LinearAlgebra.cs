namespace Factoring
{
    class LinearAlgebra
    {
        // find X such that MX = 0 over Z2
        // M is a m x n matrix
        // d is a n dimension vector
        // r is the returned rank

        public void KernelOverZ2(int m, int n, int[] d, ref sbyte[,] M, ref int r)
        {
            sbyte D, Mis;
            int i, j, k, s;
            int[] c = new int[m];

            r = 0;

            for (i = 0; i < m; i++)
                c[i] = -1;

            for (k = 0; k < n; k++)
            {

                for (j = 0; j < m; j++)
                    if (M[j, k] != 0 && c[j] == -1)
                        break;

                if (j < m)
                {
                    D = -1;
                    M[j, k] = -1;

                    for (s = k + 1; s < n; s++)
                        M[j, s] = (sbyte)(D * M[j, s]);

                    for (i = 0; i < m; i++)
                    {

                        if (i != j)
                        {
                            D = M[i, k];
                            M[i, k] = 0;

                            for (s = k + 1; s < n; s++)
                            {
                                Mis = (sbyte)((M[i, s] + D * M[j, s]) % 2);

                                if (Mis < 0)
                                    Mis += 2;

                                M[i, s] = Mis;
                            }
                        }
                    }

                    c[j] = k;
                    d[k] = j;
                }

                else
                {
                    r = r + 1;
                    d[k] = -1;
                }
            }
        }
    }
}