using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab4
{
    internal class SpecificActors : DataForFormulas
    {
        public static List<int> Actors(ulong num)
        {
            string sixBase = ConvertBase($"{num}", 10, dimension);
            Console.WriteLine($"sixbase = {sixBase}\tnum = {num}\t% = {num / Math.Pow(dimension, N):0.000}");
            var listBigA = ValuesA(sixBase, N);
            return listBigA;
        }

        private static string ConvertBase(string number, int fromBase, int toBase) //перевод числа в нужную систему счисления
        {
            // Проверка корректности аргументов
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException("number");
            if (fromBase < 2 || fromBase > 36)
                throw new ArgumentOutOfRangeException("fromBase");
            if (toBase < 2 || toBase > 36)
                throw new ArgumentOutOfRangeException("toBase");

            // Инициализация массива цифр и словаря для преобразования
            char[] digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Dictionary<char, int> digitValues = new Dictionary<char, int>();
            for (int i = 0; i < digits.Length; i++)
                digitValues.Add(digits[i], i);

            // Перевод числа из исходной системы в десятичную
            long decimalValue = 0;
            for (int i = 0; i < number.Length; i++)
            {
                char c = number[i];
                int digitValue;
                if (!digitValues.TryGetValue(c, out digitValue) || digitValue >= fromBase)
                    throw new ArgumentException("Invalid character in the number", "number");
                decimalValue = decimalValue * fromBase + digitValue;
            }

            // Перевод числа из десятичной системы в целевую
            string result = "";
            do
            {
                int remainder = (int)(decimalValue % toBase);
                result = digits[remainder] + result;
                decimalValue /= toBase;
            }
            while (decimalValue > 0);

            return result;
        }

        private static List<int> ValuesA(string num, int specN) //значения A в списке
        {
            List<int> valuesA = new List<int>();

            if (num.Length < specN)
            {
                num = string.Concat(Enumerable.Repeat("0", specN - num.Length)) + num;
            }

            if (num.Length == specN)
            {
                for (int i = 0; i < specN; i++)
                {
                    switch (num[i])
                    {
                        case '0':
                            valuesA.Add(-3);
                            break;
                        case '1':
                            valuesA.Add(-2);
                            break;
                        case '2':
                            valuesA.Add(-1);
                            break;
                        case '3':
                            valuesA.Add(1);
                            break;
                        case '4':
                            valuesA.Add(2);
                            break;
                        case '5':
                            valuesA.Add(3);
                            break;
                        default:
                            throw new Exception("dimensionIsWrong");
                    }
                }
            }
            else
                throw new Exception("lengthIsWrong");

            return valuesA;
        }
    }
}
