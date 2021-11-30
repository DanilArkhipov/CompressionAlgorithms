using Fano.Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Fano
{
    public class DataReader
    {

        /// <summary>
        /// Метод считывает данные для кодирования из файла
        /// </summary>
        public IEnumerable<string> ReadDataChunk(string path)
        {
            return File.ReadLines(path);
        }

        /// <summary>
        /// Метод записывает закодированные данные в файл
        /// </summary>
        public void WriteEncodedData(string path, FanoEncodedData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, data);
            }
        }

        /// <summary>
        /// Меод считывает закодированные данные
        /// </summary>
        public FanoEncodedData ReadEncodedData(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                FanoEncodedData data = (FanoEncodedData)formatter.Deserialize(fs);
                return data;
            }
        }

        /// <summary>
        /// Метод пишет данные в файл
        /// </summary>
        public void WriteDataToFile(string path, string text)
        {
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
        }
    }
}
