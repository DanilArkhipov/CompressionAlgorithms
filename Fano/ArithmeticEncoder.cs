using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class ArithmeticEncoder : ICoder
    {
        public async Task Decode(string inputPath, string outputPath)
        {
            ArithmeticAlgorithm.Decode(inputPath, outputPath);
        }

        public async Task Encode(string inputPath, string outputPath)
        {
            ArithmeticAlgorithm.Encode(inputPath, outputPath);
        }
    }
}
