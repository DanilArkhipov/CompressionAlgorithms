using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class ShennonFanoCoder : ICoder
    {
        public async Task Decode(string inputPath, string outputPath)
        {
            ShennonFanoAlgorithm.Decode(inputPath, outputPath);
        }

        public async Task Encode(string inputPath, string outputPath)
        {
            ShennonFanoAlgorithm.Encode(inputPath, outputPath);
        }
    }
}
