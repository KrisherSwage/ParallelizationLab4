using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class CalculationE : DataForFormulas
    {
        public static double CalculateE(List<int> listBigA)
        {
            double result = 0.0;

            result += AdditivityA(listBigA);

            result += RelationshipA(listBigA);

            return result;
        }

        private static double AdditivityA(List<int> bigA)
        {
            double intermediateResult = 0.0;
            for (int i = 0; i < N; i++)
            {
                intermediateResult += coefA[i] * bigA[i];
            }
            return intermediateResult;
        }

        private static double RelationshipA(List<int> bigA)
        {
            double intermediateResult = 0.0;
            int itrForB = 0;
            for (int i = 0; i < N - 1; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    intermediateResult += coefB[itrForB] * bigA[i] * bigA[j];
                    itrForB++;
                }
            }
            return intermediateResult;
        }
    }
}
