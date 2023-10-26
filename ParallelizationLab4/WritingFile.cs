using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class WritingFile
    {
        public static void CriterionNash(string fileName)
        {
            var iE = NashEquilibrium.ConfigurationE();
            var nashCrit = NashEquilibrium.NashCriter();

            string pathWriteData = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");
            using (StreamWriter sw = new StreamWriter(pathWriteData, false, Encoding.UTF8))
            {
                sw.WriteLine("E;Nash;");

                for (int i = 0; i < 1000; i += 1)
                {
                    sw.WriteLine($"{iE[i]};{nashCrit[i]};");
                }
                for (int i = 1000; i < iE.Count; i += 200)
                {
                    sw.WriteLine($"{iE[i]};{nashCrit[i]};");
                }
            }
        }

        public static void AvergeTimes(List<List<double>> myList, string fileName)
        {
            string pathWriteData = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");

            using (StreamWriter sw = new StreamWriter(pathWriteData, false, Encoding.UTF8))
            {
                for (int i = 0; i < myList.Count; i++)
                {
                    sw.Write($"{i + 1};");
                    for (int j = 0; j < myList[i].Count; j++)
                    {
                        sw.Write($"{myList[i][j]};");
                    }
                    sw.WriteLine();
                }
            }
        }

        public static void DifferentData(List<string> myList, string fileName)
        {
            string pathWriteData = Path.Combine(Environment.CurrentDirectory, $"{fileName}.txt");

            using (StreamWriter sw = new StreamWriter(pathWriteData, true, Encoding.UTF8))
            {
                for (int i = 0; i < myList.Count - 1; i++)
                {
                    sw.WriteLine($"{myList[i]}");
                }
                sw.WriteLine($"{myList[myList.Count - 1]}");
                sw.WriteLine($"---");
            }
        }

        public static void Сoefficients(int counter, string fileName) //метод на один раз
        {
            //int n = 24;
            //Сoefficients(n, "coeff_a");
            //Сoefficients((n * n - n) / 2, "coeff_b");
            string pathWriteData = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");

            using (StreamWriter sw = new StreamWriter(pathWriteData, false, Encoding.UTF8))
            {
                for (int i = 0; i < counter; i++)
                {
                    sw.WriteLine($"{СoefficientGeneration()};");
                }
            }
        }
        private static Random rnd = new Random();
        private static double СoefficientGeneration() //для коэффициетов a и b (на один раз)
        {
            double res = rnd.NextDouble() * (1.0 - (-1.0)) + (-1.0);
            return res;
        }

    }
}
