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
            byte[] data = File.ReadAllBytes(inputPath);
            var str = ArithmeticAlgorithm.Decode(data);
            await Task.CompletedTask;
        }

        public async Task Encode(string inputPath, string outputPath)
        {
            string text;
            using (var sr = new StreamReader(inputPath))
            {
                text = await sr.ReadToEndAsync();
            }
            var data =  ArithmeticAlgorithm.Encode(text);

            using (var bw = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                bw.Write(data);
            }
        }
    }
}
