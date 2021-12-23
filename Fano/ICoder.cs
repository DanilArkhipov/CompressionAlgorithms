using Algorithms.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public interface ICoder
    {
        /// <summary>
        /// Закодировать исходный текст
        /// </summary>
        Task<EncodedData> Encode(IEnumerable<string> text);

        /// <summary>
        /// Дескодировать текст
        /// </summary>
        string Decode(EncodedData data);
    }
}
