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
        Task Encode(String inputPath, String outputPath);

        /// <summary>
        /// Дескодировать текст
        /// </summary>
        Task Decode(String inputPath, String outputPath);
    }
}
