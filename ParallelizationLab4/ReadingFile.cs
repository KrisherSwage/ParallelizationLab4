using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class ReadingFile
    {
        public static double SpecificEmax(string path) //чтение файла для одного максимального E
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                if ((line = reader.ReadLine()) != null)
                {
                    return Convert.ToDouble(line);
                }
                else
                {
                    throw new Exception("readingErrorE");
                }
            }
        }

        public static List<double> CoeffFromFile(string path) //чтение файла для коэффициентов a и b 
        {
            List<double> lines = new List<double>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(Convert.ToDouble(line));
                }
            }
            return lines;
        }

    }
}
