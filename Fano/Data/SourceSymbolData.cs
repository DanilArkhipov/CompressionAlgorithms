using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano.Data
{
    /// <summary>
    /// Данные о символах в исходном тексте
    /// </summary>
    public class SourceSymbolData
    {
        /// <summary>
        /// Символ в исходном алфавите
        /// </summary>
        public char SymbolInSourceAlphabet { get; }

        /// <summary>
        /// Частота символа в тексте
        /// </summary>
        public int Frequency { get; set; }

        public SourceSymbolData(char symbol, int frequency)
        {
            SymbolInSourceAlphabet = symbol;
            Frequency = frequency;
        }
    }
}
