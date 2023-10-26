using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ParallelizationLab4
{
    internal class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            //ulong d = 789730223053602816;
            //Console.WriteLine(SpecificActors.ConvertBase($"789730223053602816", 10, 6)); //вернет: 100.000.000.000.000.000.000.000 = 6^23(10 сс)
            //ulong dd = Convert.ToUInt64(Math.Pow(6, 23));
            //Console.WriteLine(dd); //это корректно

            //эксперимент для засечки времени
            List<List<double>> averageTimes = new List<List<double>>();
            for (int i = 0; i < 12; i++)
            {
                DataForFormulas.fluxes = i + 1;
                DataForFormulas.InitialConditions();

                averageTimes.Add(new List<double>());

                for (int j = 0; j < 51; j++)
                {
                    var averageTimeResult = CodeParallelization.StartParalCalc();
                    averageTimes[i].Add(averageTimeResult);
                }
            }
            WritingFile.AvergeTimes(averageTimes, "Average Times");

            //если нужен единственный запуск программы
            //DataForFormulas.fluxes = 12;
            //DataForFormulas.InitialConditions();
            //var averageTimeResult = CodeParallelization.StartParalCalc();

            //критерий Нэша запускаем один раз
            //DataForFormulas.fluxes = 1;
            //DataForFormulas.InitialConditions();
            //NashEquilibrium.BeginNash();
            //WritingFile.CriterionNash("Nash");

            Console.WriteLine("Программа завершена");
            Console.ReadLine();
        }


    }


}
