using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.Data.Enums
{
    public static class EnumHelper
    {
        public static string GetRussianAlgorithmName(string englishName)
        {
            if(englishName == Algorithm.Hamming.ToString())
            {
                return "Код Хэмминга";
            }

            if (englishName == Algorithm.ShennonFano.ToString())
            {
                return "Алгоритм Шеннона-Фано";
            }
            if (englishName == Algorithm.Arithmetic.ToString())
            {
                return "Арфиметическое кодирование";
            }

            throw new ArgumentException("Некорректное название алгоритма");
        }
        public static string[] GetAlgorithmNames()
        {
            return Enum.GetNames(typeof(Algorithm))
                .Select(x => GetRussianAlgorithmName(x))
                .ToArray();
        }

        public static string GetRussianActionName(string englishName)
        {
            if (englishName == Action.Encode.ToString())
            {
                return "Закодировать";
            }

            if (englishName == Action.Decode.ToString())
            {
                return "Декодировать";
            }

            throw new ArgumentException("Некорректное название алгоритма");
        }
        public static string[] GetActionNames()
        {
            return Enum.GetNames(typeof(Action))
                .Select(x => GetRussianActionName(x))
                .ToArray();
        }
    }
}
