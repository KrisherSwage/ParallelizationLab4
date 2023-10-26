using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class MonteCarlo : CalculationE
    {
        public static double BeginE()
        {
            int taskNumber = Convert.ToInt32((Task.CurrentId - 1) % fluxes); //идентификатор потока
            double numCoef = randomsList[taskNumber].NextDouble(); //генерируем коэффициент для числа значений акторов

            ulong num = Convert.ToUInt64((Math.Round(Math.Pow(dimension, N) + 1) * numCoef)); //число для значений акторов

            /*
            //if ((Task.CurrentId - 1) % fluxes == 0) //все A равны -3 для первого потока
            //    num = 0;
            //if ((Task.CurrentId - 1) % fluxes == fluxes - 1) //все A равны 3 для последнего потока
            //    num = Convert.ToUInt64(Math.Round(Math.Pow(dimension, N) - 1));
            */

            List<int> actorsList = SpecificActors.Actors(num); //сами значения акторов
            double firstE = CalculateE(actorsList); //получаем значение E по первому набору акторов (в одном из потоков)

            double time = SearchOptimal(firstE, actorsList);

            return time;
        }

        private static double SearchOptimal(double oldE, List<int> actorsList)
        {
            Stopwatch time = new Stopwatch(); //время
            time.Start(); //время

            int[] actors = new int[actorsList.Count];
            Array.Copy(actorsList.ToArray(), actors, actorsList.Count); //ну пусть будет

            int taskNumber = Convert.ToInt32((Task.CurrentId - 1) % fluxes); //номер для обращения в разных потоках

            double maxEinTask = -10000000.0; //максимальное E в потоке

            int iterIdenticalE = 0;
            int iterDifferentE = 0;

            double T = 0.8 * (taskNumber + 1); //параметр неопределенности

            int numGlobIter = 10000;
            int iteratorForMaxEinTask = 0;
            //for (int manyTimes = 0; manyTimes < 2000000; manyTimes++)
            while (flagMaximum && (iteratorForMaxEinTask < numGlobIter))
            {
                iteratorForMaxEinTask++;
                for (int i = 0; i < actors.Length; i++)
                {
                    //Часть с T
                    if (iterIdenticalE > 5000)
                    {
                        T *= 4;
                        if (T > 500)
                            T /= 20;
                        iterIdenticalE = 0;
                    }
                    if (iterDifferentE > 5000)
                    {
                        T /= 4;
                        if (T < 0.1)
                            T *= 20;
                        iterDifferentE = 0;
                    }
                    if (iteratorForMaxEinTask % 1000 == 0)
                    {
                        T = 100000;
                    }

                    //Основная часть
                    List<double> newEs = new List<double>(); //список для 1й или 2х новых E

                    int oldConf = actors[i]; //изначальная конфигурация актора
                    List<int> newConfig = new List<int>(); //список для 1й или 2х новых конфигураций акторов

                    int a; //буфферная переменная
                    if ((a = SingleConfiguration(oldConf, true)) != 0) //сначала на увеличение
                    {
                        actors[i] = a;
                        newEs.Add(CalculateE(new List<int>(actors)));
                        newConfig.Add(a);
                    }
                    if ((a = SingleConfiguration(oldConf, false)) != 0) //потом на уменьшение
                    {
                        actors[i] = a;
                        newEs.Add(CalculateE(new List<int>(actors)));
                        newConfig.Add(a);
                    }

                    actors[i] = oldConf;

                    int fOrS = 0; //какую из конфигураций выбирать
                    if (newConfig.Count == 2)
                    {
                        if (newEs[0] <= newEs[1])
                        {
                            fOrS = 1;
                        }
                    }

                    double maxNewE = newEs.Max(); //выбираю максимум из 2х новых E

                    double deltaE = maxNewE - oldE; //разница максимума и старой E

                    if (deltaE > 0)
                    {
                        //условие принятия
                        oldE = maxNewE;
                        if (newConfig[fOrS] != oldConf)
                            actors[i] = newConfig[fOrS];
                        else
                            throw new Exception("deltaE > 0"); //для отладки
                        iterIdenticalE++; //для изменения T
                    }
                    else
                    {
                        //если не приняли, то работаем с параметром неопределенности
                        double P = Math.Exp(deltaE / T);
                        double R = randomsList[taskNumber].NextDouble();

                        if (P >= R)
                        {
                            oldE = maxNewE;
                            if (newConfig[fOrS] != oldConf)
                                actors[i] = newConfig[fOrS];
                            else
                                throw new Exception("P >= R"); //для отладки
                            iterIdenticalE++; //для изменения T
                        }
                        else
                        {
                            actors[i] = oldConf; //когда ничего не принимаем
                            iterDifferentE++; //для изменения T
                        }
                    }

                    //Часть проверки результата
                    //if ((int)Math.Round(maxNewE) == (int)Math.Round(oldE)) //для изменения параметра неопределенности
                    //    iterIdenticalE++;
                    //else
                    //    iterDifferentE++;

                    if (maxEinTask < oldE) //поиск максимального значения в потоке
                        maxEinTask = oldE;

                    if (Math.Abs(maxEinTask - MaximumE) < 1e-99) //условие выхода из цикла во всех потоках
                    {
                        flagMaximum = false;

                        /* Для отладки
                        Console.WriteLine($"id = {taskNumber} Чему равны A итоговые:");
                        for (int u = 0; u < actors.Length; u++)
                        {
                            Console.Write($"{actors[u]}, ");
                        }
                        Console.WriteLine();

                        //bool actorsIsEqual = Enumerable.SequenceEqual(actors, new int[] { 3, -3, -3, -3, 3, 3, -3, -3, 3, 3 }); //для N=10
                        bool actorsIsEqual = Enumerable.SequenceEqual(actors, new int[] { -3, -3, 3, 3, -3, -3, 3, 3, -3, 3, 3, -3, 3, -3, 3, 3, -3, 3, -3, 3, -3, 3, -3, 3 }); //для N=24
                        if (actorsIsEqual)
                            Console.ForegroundColor = ConsoleColor.Green; // устанавливаем цвет
                        else
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nОдинаковый ли ответ - {actorsIsEqual}\n");
                        Console.ResetColor(); // сбрасываем в стандартный
                        */

                        if (maxEinTask > MaximumE)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nmaxEinTask (>) = {maxEinTask.ToString("R")}");
                            Console.ResetColor();
                        }

                        break;
                    }
                }
            }

            time.Stop(); //время
            double myTime = time.ElapsedMilliseconds / 1000.0; //время
            Console.WriteLine($"время = {myTime} сек;\tкакое Emax нашли = {maxEinTask.ToString("R")};\tID = {Task.CurrentId}"); //время

            if (iteratorForMaxEinTask == numGlobIter)
            {
                Console.WriteLine("Не досчитали");
                myTime = 20.0;
            }
            return myTime;
        }

        private static int SingleConfiguration(int oldConf, bool increase)
        {
            if (increase)
            {
                if (oldConf == 3)
                    return 0;
                if (oldConf == -1)
                    return 1;
                return oldConf + 1;
            }
            else
            {
                if (oldConf == -3)
                    return 0;
                if (oldConf == 1)
                    return -1;
                return oldConf - 1;
            }
        }
    }
}
