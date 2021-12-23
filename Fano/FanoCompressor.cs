using Algorithms.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithms.Comparers;


namespace Algorithms
{
    public class FanoCompressor 
    {
        public FanoDataPreparer DataPreparer { get; set; }

        public FanoCompressor()
        {
            DataPreparer = new FanoDataPreparer();
        }
        public string Decode(EncodedData data)
        {
            var res = new StringBuilder();
            var codes = new Dictionary<BitArray, char>(data.MetaDada.Codes.Length, new BitArrayComparer());
            foreach (var c in data.MetaDada.Codes)
            {
                codes.Add(c.Code, c.Symbol);
            }
            var code = new BitArray(1);
            int localCodePos = 0;
            char value;
            for (int i = 0; i < data.Data.Length; i++)
            {
                code[localCodePos] = data.Data[i];
                if (codes.TryGetValue(code, out value))
                {
                    res.Append(value);
                    code = new BitArray(1);
                    localCodePos = 0;
                }
                else
                {
                    code.Length += 1;
                    localCodePos += 1;
                }
            }
            return res.ToString();
        }

        public async Task<EncodedData> Encode(IEnumerable<string> text)
        {
            var sourceSymbolsData = await DataPreparer.PrepareDataAsync(text);
            var codes = CreateCodes(sourceSymbolsData);

            int bitsCount = 0;
            foreach (var s in sourceSymbolsData)
            {
                bitsCount += s.Frequency * codes[s.SymbolInSourceAlphabet].Length;
            }
            var encodedText = new BitArray(bitsCount);

            int currPos = 0;
            BitArray bits;
            foreach (var line in text)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    bits = codes[line[i]];
                    for (int j = 0; j < bits.Count; j++)
                    {
                        encodedText[currPos] = bits[j];
                        currPos++;
                    }
                }
            }
            var symbolCodes = codes.Select(x => new SymbolCode() { Symbol = x.Key, Code = x.Value }).ToArray();
            var metaData = new FanoMetaData(symbolCodes);
            return new EncodedData(metaData, encodedText);
        }

        /// <summary>
        /// Метод, который сопоставляет коды для символов алфавита
        /// </summary>
        private Dictionary<char, BitArray> CreateCodes(ICollection<SourceSymbolData> preparedData)
        {
            var codes = new Dictionary<char, BitArray>();
            var sortedData = preparedData
                .OrderByDescending(x => x.Frequency)
                .ToArray();
            CreateFanoCodeTree(codes, sortedData, 0, sortedData.Length, new BitArray(0));
            return codes;
        }

        /// <summary>
        /// Строит дерево кодов Фано и заполняет словарь, передаваемый первым параметром, полученными кодами
        /// </summary>
        private void CreateFanoCodeTree(Dictionary<char, BitArray> codes, SourceSymbolData[] data, int startIndex, int endIndex, BitArray currentCode)
        {
            var splitBorder = FindSplitBorder(data, startIndex, endIndex);

            if (splitBorder - startIndex == 1 && endIndex - splitBorder == 1)
            {
                AddCodeToDictionary(codes, data[startIndex].SymbolInSourceAlphabet, AddCodeValue(currentCode, false));
                AddCodeToDictionary(codes, data[splitBorder].SymbolInSourceAlphabet, AddCodeValue(currentCode, true));
            }
            else if (splitBorder - startIndex == 1)
            {
                AddCodeToDictionary(codes, data[startIndex].SymbolInSourceAlphabet, AddCodeValue(currentCode, false));
                CreateFanoCodeTree(codes, data, splitBorder, endIndex, AddCodeValue(currentCode, true));
            }
            else if (endIndex - splitBorder == 1)
            {
                AddCodeToDictionary(codes, data[splitBorder].SymbolInSourceAlphabet, AddCodeValue(currentCode, true));
                CreateFanoCodeTree(codes, data, startIndex, splitBorder, AddCodeValue(currentCode, false));
            }
            else
            {
                CreateFanoCodeTree(codes, data, startIndex, splitBorder, AddCodeValue(currentCode, false));
                CreateFanoCodeTree(codes, data, splitBorder, endIndex, AddCodeValue(currentCode, true));
            }

        }

        /// <summary>
        /// Метод безопасного добавления кода в словарь
        /// </summary>
        private void AddCodeToDictionary(Dictionary<char, BitArray> dict, char key, BitArray value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// Метод находит границу, которая делит упорядоченную по вероятностям коллекцию на две части с одинаковыми суммами вероятностей
        /// </summary>
        private int FindSplitBorder(SourceSymbolData[] data, int startIndex, int endIndex)
        {
            var leftQuantity = 0;
            var rightQuantity = 0;
            var difference = int.MaxValue;
            var newDifference = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                leftQuantity += data[i].Frequency;
                rightQuantity = 0;

                for (int j = i + 1; j < endIndex; j++)
                {
                    rightQuantity += data[j].Frequency;
                }

                if (i == startIndex)
                {
                    difference = Math.Abs(rightQuantity - leftQuantity);
                    continue;
                }

                newDifference = Math.Abs(rightQuantity - leftQuantity);
                if (newDifference > difference)
                {
                    return i;
                }
                difference = newDifference;
            }
            return endIndex - 1;
        }

        /// <summary>
        /// Метод добавляющий в конец текущего кодв новое значение переданное в параметр
        /// </summary>
        private BitArray AddCodeValue(BitArray currentCode, bool newValue)
        {
            var newCode = new BitArray(currentCode.Length + 1);
            for (int i = 0; i < currentCode.Length; i++)
            {
                newCode[i] = currentCode[i];
            }
            newCode[currentCode.Length] = newValue;
            return newCode;
        }
    }
}
