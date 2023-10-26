using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class CodeParallelization
    {
        public static double StartParalCalc()
        {

            DataForFormulas.flagMaximum = true;
            int fl = DataForFormulas.fluxes;

            Task<double>[] tasks = new Task<double>[fl]; //массив для созданных потоков
            for (int i = 0; i < fl; i++)
            {
                tasks[i] = Task.Run(() => MonteCarlo.BeginE()); //создаем и запускаем новый поток с функцией расчета
            }

            Task.WaitAll(tasks);

            double arithmeticalMean = 0.0;
            //List<string> result = new List<string>();
            for (int i = 0; i < fl; i++)
            {
                //result.Add($"{tasks[i].Result}");
                //Console.WriteLine($"tasks[i].Result = {tasks[i].Result}");
                arithmeticalMean += tasks[i].Result;
            }

            arithmeticalMean /= fl;

            return arithmeticalMean;
        }

    }
}
