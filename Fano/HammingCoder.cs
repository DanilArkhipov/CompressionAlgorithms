using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class HammingCoder : ICoder
    {
        public async Task Decode(string inputPath, string outputPath)
        {
            var code = File.ReadAllText(inputPath);
            var decoded = HammingAlgorithm.Decode(code);
            File.WriteAllText(outputPath, decoded);
        }

        public async Task Encode(string inputPath, string outputPath)
        {
            var code = File.ReadAllText(inputPath);
            var encoded = HammingAlgorithm.Encode(code);
            File.WriteAllText(outputPath, encoded);
        }
    }
}
