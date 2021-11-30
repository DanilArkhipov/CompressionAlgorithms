using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano.Data
{
    /// <summary>
    /// Класс содержащий данные о символе исходного алфавита и его коде
    /// </summary>
    [Serializable]
    public class SymbolCode
    {
        /// <summary>
        /// Символ
        /// </summary>
        public char Symbol { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public BitArray Code { get; set; }
    }
}
