using Algorithms.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithms.Comparers;
using System.IO;

namespace Algorithms
{
    public static class ShennonFanoAlgorithm
    {
        public static void Decode(string input, string output)
        {
            byte symbolsCount;
            List<SourceSymbolData> symbols = new List<SourceSymbolData>();
            int bitsCount;
            byte[] bytes;

            using (var br = new BinaryReader(File.OpenRead(input)))
            {
                symbolsCount = br.ReadByte();
                for(int i = 0; i < symbolsCount; i++)
                {
                    symbols.Add(new SourceSymbolData(br.ReadChar(), br.ReadInt32()));
                }
                bitsCount = br.ReadInt32();
                var bytesCount = bitsCount % 8 == 0 ? bitsCount / 8 : (bitsCount / 8) + 1;
                bytes = br.ReadBytes(bytesCount);
            }

            var encodeCodes = CreateCodes(symbols);
            var bits = GetBits(bytes, bitsCount);
            bits.Length = bitsCount;
            var res = new StringBuilder();
            var codes = new Dictionary<BitArray, char>(encodeCodes.Count, new BitArrayComparer());
            foreach (var c in encodeCodes)
            {
                codes.Add(c.Value, c.Key);
            }
            var code = new BitArray(1);
            int localCodePos = 0;
            char value;
            for (int i = 0; i < bits.Length; i++)
            {
                code[localCodePos] = bits[i];
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
            
            File.WriteAllText(output,res.ToString());
        }

        public static void Encode(string input, string output)
        {
            var text = File.ReadAllText(input);
            var sourceSymbolsData = GetSourceSymbolsData(text);
            var codes = CreateCodes(sourceSymbolsData);

            int bitsCount = 0;
            foreach (var s in sourceSymbolsData)
            {
                bitsCount += s.Frequency * codes[s.SymbolInSourceAlphabet].Length;
            }
            var encodedText = new BitArray(bitsCount);

            int currPos = 0;
            BitArray bits;

            for (int i = 0; i < text.Length; i++)
            {
                bits = codes[text[i]];
                for (int j = 0; j < bits.Count; j++)
                {
                    encodedText[currPos] = bits[j];
                    currPos++;
                }
            }

            using (var bw = new BinaryWriter(File.Open(output,FileMode.Create)))
            {
                bw.Write((byte)sourceSymbolsData.Length);
                foreach (var data in sourceSymbolsData)
                {
                    bw.Write(data.SymbolInSourceAlphabet);
                    bw.Write(data.Frequency);
                }
                var bytes = BitsToBytes(encodedText);
                bw.Write(encodedText.Length);
                bw.Write(bytes);
            }
        }

        private static byte[] BitsToBytes(BitArray bits)
        {
            var bytes = new List<Byte>();
            int currentByte = 0;
            int currentMask = 128;
            int j = 0;
            for (int i = 0;i < bits.Count; i++)
            {
                if (bits[i] == true)
                {
                    currentByte |= currentMask;
                }
                j++;
                currentMask >>= 1;
                if (j == 8)
                {
                    j = 0;
                    bytes.Add((byte)currentByte);
                    currentMask = 128;
                    currentByte = 0;
                }
            }
            if (bits.Length % 8 != 0)
            {
                bytes.Add((byte)currentByte);
            }
            return bytes.ToArray();
        }

        private static BitArray GetBits(byte[] bytes, int countOfBits)
        {
            var sb = new StringBuilder();
            var str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str = Convert.ToString(bytes[i], toBase: 2);
                if (str.Length < 8)
                {
                    str = new string('0', 8 - str.Length) + str;
                }
                sb.Append(str);
            }
            str = sb.ToString().Substring(0, countOfBits);
            var bits = new BitArray(countOfBits);

            for(int i = 0; i < bits.Length; i++)
            {
                bits[i] = str[i].ToString() == "1";
            }
            return bits;
        }

        /// <summary>
        /// Метод, который сопоставляет коды для символов алфавита
        /// </summary>
        private static Dictionary<char, BitArray> CreateCodes(ICollection<SourceSymbolData> preparedData)
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
        private static void CreateFanoCodeTree(Dictionary<char, BitArray> codes, SourceSymbolData[] data, int startIndex, int endIndex, BitArray currentCode)
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
        private static void AddCodeToDictionary(Dictionary<char, BitArray> dict, char key, BitArray value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// Метод находит границу, которая делит упорядоченную по вероятностям коллекцию на две части с одинаковыми суммами вероятностей
        /// </summary>
        private static int FindSplitBorder(SourceSymbolData[] data, int startIndex, int endIndex)
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
        /// Метод добавляющий в конец текущего кода новое значение переданное в параметр
        /// </summary>
        private static BitArray AddCodeValue(BitArray currentCode, bool newValue)
        {
            var newCode = new BitArray(currentCode.Length + 1);
            for (int i = 0; i < currentCode.Length; i++)
            {
                newCode[i] = currentCode[i];
            }
            newCode[currentCode.Length] = newValue;
            return newCode;
        }

        private static SourceSymbolData[] GetSourceSymbolsData(string text)
        {
            var dict = new Dictionary<Char, SourceSymbolData>();

            foreach (char c in text)
            {
                if (!dict.ContainsKey(c))
                {
                    dict.Add(c, new SourceSymbolData(c, 1));
                }
                else
                {
                    dict[c].Frequency += 1;
                }
            }
            return dict.Values.ToArray();
        }
    }
}
