using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;


namespace Algorithms
{
    public static class LZWAlgorithm
    {
        public static void Encode(string input, string output)
        {
            var text = File.ReadAllText(input);
            var dict = GetInitialDictionary(text);
            var chars = new String(dict.Keys.Select(i => i[0]).ToArray());
            var codes = GetCodes(text, dict);
            var bitsPerSymbol = (int)(Math.Log(dict.Count, 2) + 0.999);
            var bits = GetBinaryView(codes, bitsPerSymbol);
            var bytes = GetBytesFromBits(bits);           
            WriteData(output, chars,bytes, bitsPerSymbol,bits.Length);
        }
        private static List<int> GetCodes(string text, Dictionary<string,int> dict)
        {
            var codes = new List<int>();
            var currentString = "";
            var prevString = "";
            var count = dict.Values.Count;
            for (int i = 0; i < text.Length; i++)
            {
                prevString = currentString;
                currentString += text[i];
                if (!dict.ContainsKey(currentString))
                {
                    codes.Add(dict[prevString]);
                    dict.Add(currentString, count);
                    count++;
                    currentString = text[i].ToString();
                }
            }
            codes.Add(dict[currentString]);
            return codes;
        }
        private static Dictionary<string, int> GetInitialDictionary(string text)
        {
            var numberOfUniqueSymbol = 0;
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (!dict.ContainsKey(text[i].ToString()))
                {
                    dict.Add(text[i].ToString(), numberOfUniqueSymbol);
                    numberOfUniqueSymbol++;
                }
            }
            return dict;
        }

        private static string GetBinaryView(List<int> codes, int BitsPerSymbol)
        {
            var sb = new StringBuilder();
            var tmpStr = "";
            foreach (var code in codes)
            {
                tmpStr = Convert.ToString(code, toBase: 2);
                if (tmpStr.Length < BitsPerSymbol)
                {
                    tmpStr= new String('0', BitsPerSymbol - tmpStr.Length)+ tmpStr;
                }
                sb.Append(tmpStr);
            }
            return sb.ToString();
        }

        private static byte[] GetBytesFromBits(string bits)
        {
            var bytes = new List<Byte>();
            int currentByte = 0;
            int currentMask = 128;
            int j = 0;
            foreach (var bit in bits)
            {
                if (bit.ToString() == "1")
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

        private static void WriteData(string outputPath, string chars, byte[] bytes, int bitsPerSymbol, int bitsCount)
        {
            using (var bw = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                bw.Write(chars.Length);
                for (int i = 0; i < chars.Length; i++)
                {
                    bw.Write(chars[i]);
                }
                bw.Write(bitsPerSymbol);
                bw.Write(bitsCount);
                bw.Write(bytes.Length);
                bw.Write(bytes);
            }
        }

        private static void ReadData(string inputPath, ref string chars, ref byte[] bytes, ref int bitsPerSymbol, ref int bitsCount)
        {
            var countOfChars = 0;
            var countOfBytes = 0;
            var sb = new StringBuilder();
            using (var br = new BinaryReader(File.OpenRead(inputPath)))
            {
                countOfChars = br.ReadInt32();
                for (int i = 0; i < countOfChars; i++)
                {
                    sb.Append(br.ReadChar());
                }
                chars = sb.ToString();
                bitsPerSymbol = br.ReadInt32();
                bitsCount = br.ReadInt32();
                countOfBytes = br.ReadInt32();
                bytes = br.ReadBytes(countOfBytes);
            }
        }

        public static void Decode(string input, string output)
        {
            string chars = "";
            byte[] bytes = null;
            int bitsPerSymbol = 0;
            int bitsCount = 0;
            ReadData(input,ref chars,ref bytes, ref bitsPerSymbol,ref bitsCount);
            var dict = GetDecodeDictionary(chars);
            var bits = GetBits(bytes, bitsCount);
            var codes = GetCodes(bits,bitsPerSymbol);
            var text = GetText(dict, codes);
            File.WriteAllText(output,text);
        }

        private static List<int> GetCodes(string bits, int bitsPerSymbol)
        {
            var codes = new List<int>();
            var tmp = "";
            for(int i = 0; i <bits.Length/bitsPerSymbol; i++)
            {
                tmp = bits.Substring(i * bitsPerSymbol, bitsPerSymbol);
                codes.Add(Convert.ToInt32(tmp, fromBase: 2));
            }
            return codes;
        }

        private static string GetText(Dictionary<int, string> dict, List<int> codes)
        {
            var sb = new StringBuilder();

            string newString = "";
            string currentString = "";
            int number;
            int j = dict.Count;
            for(int i = 0; i < codes.Count; i++)
            {
                if (!dict.ContainsKey(codes[i]))
                {
                    dict.Add(j, currentString + currentString[0]);
                    j++;
                }

                newString = dict[codes[i]];
                sb.Append(newString);
                if (!dict.Values.Contains(currentString + newString[0]))
                {
                    dict.Add(j, currentString + newString[0]);
                    j++;
                }
                currentString = newString;
            }
            return sb.ToString();
        }

        private static Dictionary<int,string> GetDecodeDictionary(string chars)
        {
            var dict = new Dictionary<int,string>();
            for(int i = 0; i< chars.Length;i++)
            {
                dict.Add(i, chars[i].ToString());
            }
            return dict;
        }

        private static string GetBits(byte[] bytes, int countOfBits)
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
            return sb.ToString().Substring(0,countOfBits);
        }
    }
}
