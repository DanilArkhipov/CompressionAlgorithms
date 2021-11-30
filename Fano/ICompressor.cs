using Fano.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano
{
    public interface ICompressor
    {
        /// <summary>
        /// Закодировать (сжать) исходный текст
        /// </summary>
        Task<FanoEncodedData> Encode(IEnumerable<string> text);

        /// <summary>
        /// Раскодировать (восстановить) текст
        /// </summary>
        string Decode(FanoEncodedData data);
    }
}
