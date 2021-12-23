using Algorithms;
using Fano;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CompressionAlgorithms
{
    class Program
    {
        static  void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();

        }

        static async Task MainAsync(string[] args)
        {
            //if(args[0] == "0")
            //{
            //    var dataReader = new DataReader();
            //    var data = dataReader.ReadDataChunk(args[1]);
            //    var fanoCompressor = new FanoCompressor();
            //    var encodedData = await fanoCompressor.Encode(data);
            //    dataReader.WriteEncodedData(args[2], encodedData);
            //}
            //else if(args[0] == "1"){
            //    var dataReader = new DataReader();
            //    var data = dataReader.ReadEncodedData(args[1]);
            //}
            //else
            //{
            //    Console.WriteLine("Первым параметром нужно передать 0, если данные кодируются, и 1, если данные нужно раскодировать");
            //}
            var dataReader = new DataReader();
            var data = dataReader.ReadDataChunk("C:\\Users\\strel\\Desktop\\test.txt");
            var fanoCompressor = new FanoCompressor();
            var encodedData = await fanoCompressor.Encode(data);
            dataReader.WriteEncodedData("C:\\Users\\strel\\Desktop\\testRes.txt", encodedData);

            encodedData = dataReader.ReadEncodedData("C:\\Users\\strel\\Desktop\\testRes.txt");
            var str = fanoCompressor.Decode(encodedData);
            dataReader.WriteDataToFile("C:\\Users\\strel\\Desktop\\testResult.txt",str);
            Console.ReadKey();
        }
    }
}
