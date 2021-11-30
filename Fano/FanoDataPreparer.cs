using Fano.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano
{
    /// <summary>
    /// Класс отвечающий за подготовку данных для работы алгоритма Фано
    /// </summary>
    public class FanoDataPreparer
    {
        private ConcurrentDictionary<Char, SourceSymbolData> symbols;

        public FanoDataPreparer()
        {
            symbols = new ConcurrentDictionary<Char, SourceSymbolData>();
        }

        /// <summary>
        /// Метод асинхронно подсчитывающий частоты символов в строках
        /// </summary>
        /// <returns>Коллекция данных о символах в исходном тексте</returns>
        public async Task<ICollection<SourceSymbolData>> PrepareDataAsync(IEnumerable<string> strings)
        {
            await Task.WhenAll(strings.Select(x => Task.Run(() => CountFrequencyOfSymbolsInString(x))));
            return symbols.Values;
        }

        /// <summary>
        /// Потокобеопасный метод подсчёта частоты символов в строке
        /// </summary>
        private void CountFrequencyOfSymbolsInString(string str)
        {
            SourceSymbolData symbolData;
            for (int i = 0; i < str.Length; i++)
            {
                symbols.AddOrUpdate(str[i],new SourceSymbolData(str[i], 1),(key,value) => new SourceSymbolData(key,value.Frequency+1));
            }
        }
    }
}
