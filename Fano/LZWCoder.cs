using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class LZWCoder : ICoder
    {
        public async Task Decode(string inputPath, string outputPath)
        {
            LZWAlgorithm.Decode(inputPath, outputPath);   
        }

        public async Task Encode(string inputPath, string outputPath)
        {
            LZWAlgorithm.Encode(inputPath, outputPath);
        }
    }
}
