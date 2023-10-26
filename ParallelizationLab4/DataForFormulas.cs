using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class DataForFormulas
    {
        public const int dimension = 6;
        public static int N = 24;
        public static int fluxes;

        /// <summary>
        /// Флаг выхода из всех потоков
        /// </summary>
        public static bool flagMaximum = false;

        public static List<double> coefA = ReadingFile.CoeffFromFile("coeff_a.txt");
        public static List<double> coefB = ReadingFile.CoeffFromFile("coeff_b.txt");

        public static double MaximumE = ReadingFile.SpecificEmax("specificEmax.txt");

        public static List<Random> randomsList = new List<Random>();

        public static void InitialConditions()
        {
            randomsList.Clear();

            for (int i = 0; i < fluxes; i++)
            {
                randomsList.Add(new Random());
                Thread.Sleep((i + 1) * 10);
            }
        }
    }
}
