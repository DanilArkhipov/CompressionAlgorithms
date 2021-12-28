using Algorithms.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public static class ArithmeticAlgorithm
    {
        public static void Decode(string inputPath, string outPath)
        {
            Dictionary<Char, Symbol> intervals = null;
            int count = 0;
            string code = "";
            ReadData(inputPath, ref intervals, ref count, ref code);
            SetIntervals(intervals, count);
            var text = GetText(intervals, count, code);
            File.WriteAllText(outPath, text);
        }



        #region useless
        private static string GetText(Dictionary<Char, Symbol> intervals, int count, string code)
        {
            var oldIntervals = new Dictionary<Char, Symbol>();

            foreach(var v in intervals.Values)
            {
                oldIntervals.Add(v.Value, new Symbol(v.Value,v.Left,v.Right,v.Frequency));
            }
            decimal left;
            decimal right;
            string leftStr;
            string rightStr;
            StringBuilder text = new StringBuilder(count);
            for(int i = 0; i< count; i++)
            {
                foreach(var symbol in intervals.Values)
                { 
                    leftStr = symbol.Left.ToString();
                    rightStr = symbol.Right.ToString().Remove(0,2);
                    leftStr = leftStr =="0"? new string('0',rightStr.Length):leftStr.Remove(0,2);
                    left = symbol.Left;
                    right = symbol.Right;

                    if(leftStr.CompareTo(code)<=0 && code.CompareTo(rightStr) < 0)
                    {
                        text.Append(symbol.Value);                                      

                        var counOfSame = 0;
                        for (int j = 0; j < leftStr.Length; j++)
                        {
                            if (leftStr[j] == rightStr[j]) counOfSame++;
                            else break;
                        }

                        if (counOfSame > 0)
                        {                           
                            code = code.Remove(0, counOfSame);

                            left = Decimal.Parse("0,"+leftStr.Remove(0, counOfSame));
                            right = Decimal.Parse("0,"+rightStr.Remove(0, counOfSame));
                        }

                        foreach (var v in intervals.Values)
                        {
                            v.Left = Decimal.Add(left, Decimal.Multiply(Decimal.Subtract(right, left), oldIntervals[v.Value].Left));
                            v.Right = Decimal.Add(left, Decimal.Multiply(Decimal.Subtract(right, left), oldIntervals[v.Value].Right));
                        }

                        if (counOfSame > 0)
                        {
                            foreach (var v in intervals.Values)
                            {
                                left = Decimal.Parse("0," + leftStr.Remove(0, counOfSame));
                                right = Decimal.Parse("0," + rightStr.Remove(0, counOfSame));
                            }
                        }
                        break;
                    }
                }
            }
            return text.ToString();
        }

        private static void  SetIntervals(Dictionary<Char,Symbol> intervals, int count)
        {
            Decimal prev = 0.0000M;
            foreach(var v in intervals.Values)
            {
                v.Left = prev;
                prev = Decimal.Add(prev, Decimal.Divide(v.Frequency, count));
                v.Right = prev;
            }
        }

        private static ulong GetNumerator(string bits, ref int currentPos, byte countOfBits)
        {
            var sb = new StringBuilder(countOfBits);
            for (int j = 0; j < countOfBits; j++, currentPos++)
            {
                sb.Append(bits[currentPos]);
            }

            return Convert.ToUInt64(sb.ToString(), fromBase: 2);
        }
        private static byte GetCountOfBits(string bits, ref int currentPos)
        {
            var sb = new StringBuilder(6);
            for (int j = 0; j < 6; j++, currentPos++)
            {
                sb.Append(bits[currentPos]);
            }

            return (byte)(Convert.ToByte(sb.ToString(), fromBase: 2) + 1);
        }

        private static string BytesToString(byte[] data)
        {
            List<ulong> numerators = new List<ulong>();

            var sb = new StringBuilder(data.Length * 8);

            var tmpStr = "";

            for (int i = 0; i < data.Length; i++)
            {
                tmpStr = Convert.ToString(data[i], toBase: 2);

                if (8 - tmpStr.Length > 0)
                {
                    tmpStr = new string('0', 8 - tmpStr.Length) + tmpStr;
                }
                sb.Append(tmpStr);
            }
            return sb.ToString();
        }

        public static void Encode(string inputPath, string outputPath)
        {
            var text = File.ReadAllText(inputPath);
            var intervals = GetIntervals(text);
            var code = GetCode(text, intervals);
            WriteData(outputPath,intervals,code);
        }

        private static void WriteData(string outputPath,Dictionary<Char,Symbol> symbols,string code)
        {
            var symbolsValues = symbols.Values.ToArray();
            using (var bw = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                bw.Write((byte)symbolsValues.Length);

                foreach(var symbol in symbolsValues)
                {
                    bw.Write(symbol.Value);
                    bw.Write((ushort)symbol.Frequency);
                }

                var countOfZeroes = 0;

                for(int i = 0; i < code.Length; i++)
                {
                    if (code[i] == '0') countOfZeroes++;
                    else break;
                }

                var bigInt = BigInteger.Parse(code);
                var bytes = bigInt.ToByteArray();
                bw.Write((ushort)countOfZeroes);
                bw.Write((ushort)bytes.Length);
                bw.Write(bytes);
            }
        }

        private static void ReadData(string inputPath, ref Dictionary<Char, Symbol> symbols,ref int count, ref string code)
        {
            Char symbol;
            ushort frequency;
            ushort countOfZeroes;
            ushort countOfBytes;
            byte[] bytes;
            symbols = new Dictionary<char, Symbol>();
            count = 0;
            using (var br = new BinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                var countOfSymbols = br.ReadByte();

                for(int i = 0; i<countOfSymbols;i++)
                {
                    symbol = br.ReadChar();
                    frequency = br.ReadUInt16();
                    count += frequency;
                    symbols.Add(symbol,new Symbol(symbol,frequency));
                }

                countOfZeroes = br.ReadUInt16(); 
                countOfBytes = br.ReadUInt16();
                bytes = br.ReadBytes(countOfBytes);
            }
            var bigInt = new BigInteger(bytes);
            code = new string('0', countOfZeroes) + bigInt.ToString();
            
        }

        private static string GetBestInInterval(string left, string right)
        {
            return right.Substring(0, 3);
        }
        private static string GetCode(string text, Dictionary<Char,Symbol> intervals)
        {
            var sb = new StringBuilder();
            decimal oldLeft = 0;
            decimal oldRight = 1;
            decimal newLeft = 0;
            decimal newRight = 0;
            string left = "";
            string right = "";
            int countOfSame;

            for (int i = 0; i < text.Length; i++)
            {
                countOfSame = 0;
                newLeft = Decimal.Add(oldLeft, Decimal.Multiply(Decimal.Subtract(oldRight, oldLeft), intervals[text[i]].Left));
                newRight = Decimal.Add(oldLeft, Decimal.Multiply(Decimal.Subtract(oldRight, oldLeft), intervals[text[i]].Right));

                left = Convert.ToString(newLeft);
                right = Convert.ToString(newRight);
                left = left != "0" ? left.Remove(0, 2) : new string('0', right.Length);
                right = right.Remove(0, 2);

                for (int j = 0; j < left.Length; j++)
                {
                    if (left[j] == right[j])
                    {
                        countOfSame++;
                    }
                    else break;
                }

                for (int j = 0; j < countOfSame; j++)
                {
                    sb.Append(left[j]);
                }

                left = "0," + left.Remove(0,countOfSame);
                right = "0," + right.Remove(0, countOfSame);


                oldLeft = Decimal.Parse(left);
                oldRight = Decimal.Parse(right);
            }

            var best = GetBestInInterval(left, right).Remove(0, 2);
            sb.Append(best);
            return sb.ToString();
        }
        private static byte[] TransformNumeratorsToBinaryCode(ulong[] numerators)
        {
            var bytes = new List<byte>();
            byte countOfBits;
            int currentByte = 0;
            int currentMask = 128;
            int j = 0;
            var sum = 0;

            string countOfBitsBuffer;



            foreach (var numerator in numerators)
            {
                countOfBits = (byte)((Math.Log(numerator, 2) + 0.99) - 1);
                sum += countOfBits + 1 + 6;
                countOfBitsBuffer = Convert.ToString(countOfBits, toBase: 2);

                //Добавляем незначащие нули
                for (int i = 6 - countOfBitsBuffer.Length; i > 0; i--)
                {
                    countOfBitsBuffer = "0" + countOfBitsBuffer;
                }
                AddBitsToByte(ref currentByte, ref currentMask, countOfBitsBuffer, 6, bytes, ref j);

                countOfBitsBuffer = Convert.ToString(unchecked((long)numerator), toBase: 2);
                AddBitsToByte(ref currentByte, ref currentMask, countOfBitsBuffer, (byte)(countOfBits + 1), bytes, ref j);
            }

            if (bytes.Count * 8 < sum)
            {
                bytes.Add((byte)currentByte);
            }
            return bytes.ToArray();
        }

        private static void AddBitsToByte(ref int currentByte, ref int currentMask, string bits, byte bitsCount, List<byte> resultBytes, ref int j)
        {
            int i = 0;
            while (i < bitsCount)
            {
                if (bits[i] == '1')
                {
                    currentByte |= currentMask;
                }
                currentMask >>= 1;
                j++;
                i++;
                if (j == 8)
                {
                    resultBytes.Add((byte)currentByte);
                    j = 0;
                    currentByte = 0;
                    currentMask = 128;
                }
            }
        }
        //private static ulong[] CalculateNumeratorsAsync(string text)
        //{
        //    var strings = SplitBigStringToStringsWith16Length(text);

        //    var numerators = strings.Select(x => Encode16Symbols(x)).ToArray();
        //    //await Task.WhenAll(numerators);
        //    return numerators;
        //}
        //private static string[] SplitBigStringToStringsWith16Length(string text)
        //{
        //    string[] strings = null;
        //    if (text.Length % 16 == 0)
        //    {
        //        strings = new string[text.Length / 16];
        //    }
        //    else
        //    {
        //        strings = new string[(text.Length / 16) + 1];
        //    }

        //    var sb = new StringBuilder(16);

        //    for (int i = 0; i < text.Length / 16; i++)
        //    {
        //        for (int j = 0; j < 16; j++)
        //        {
        //            sb.Append(text[i * 16 + j]);
        //        }
        //        strings[i] = sb.ToString();
        //        sb.Clear();
        //    }

        //    var ostatok = text.Length % 16;
        //    if (ostatok != 0)
        //    {
        //        for (int i = 16 * (text.Length / 16); i < text.Length; i++)
        //        {
        //            sb.Append(text[i]);
        //        }
        //        strings[text.Length / 16] = sb.ToString();
        //    }

        //    return strings;
        //}
        //private static Rational GetRational(string text, Dictionary<Char,Symbol> intervals)
        //{
        //    Rational oldLeft = new Rational(0, (ulong)text.Length);
        //    Rational oldRight = new Rational((ulong)text.Length, (ulong)text.Length);
        //    Rational newLeft;
        //    Rational newRight;
        //    foreach (var symbol in text)
        //    {
        //        newRight = oldLeft + (oldRight - oldLeft) * intervals[symbol].Right;
        //        newLeft = oldLeft + (oldRight - oldLeft) * intervals[symbol].Left;
        //        oldRight = newRight;
        //        oldLeft = newLeft;
        //    }
        //    return oldLeft;

        //}
        //private static string GetText(int count, Rational rational, Symbol[] symbols)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < count; i++)
        //    {

        //        for(int j = 0; j < symbols.Length; j++)
        //        {
        //            if(symbols[j].Left<= rational &&  rational<symbols[j].Right)
        //            {
        //                sb.Append(symbols[j].Value);
        //                rational = (rational - symbols[j].Left)/(symbols[j].Right - symbols[j].Left);
        //                break;
        //            }
        //        }
        //    }
        //    return sb.ToString();
        //}

        private static Dictionary<Char, Symbol> GetIntervals(string text)
        {
            var symbols = new Dictionary<Char, Symbol>();
            foreach (char c in text)
            {
                if (!symbols.ContainsKey(c))
                {
                    symbols.Add(c, new Symbol(c, 1));
                }
                else
                {
                    symbols[c].Frequency = symbols[c].Frequency + 1;
                }
            }
            decimal prevTop = 0.0000M;
            foreach (var value in symbols.Values)
            {
                value.Left = prevTop;
                prevTop += Decimal.Divide(value.Frequency, text.Length);
                value.Right = prevTop;
            }
            return symbols;
        }

    }

    public class Symbol
    {
        private Char value;
        public Char Value => value;
        public decimal Left { get; set; }
        public decimal Right { get; set; }

        public int Frequency { get; set; }

        public Symbol(Char value, decimal left, decimal right, int frequency)
        {
            this.value = value;
            this.Left = left;
            this.Right = right;
            this.Frequency = frequency;
        }

        public Symbol(Char value, int frequency)
        {
            this.value = value;
            this.Frequency = frequency;
        }
    }
    public class Rational
    {
        public BigInteger Top { get; set; }
        public BigInteger Bottom { get; set; }

        public Rational(BigInteger top, BigInteger bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public static Rational operator *(Rational left, Rational right)
        {
            if (left.Top == 0 || right.Top == 0)
            {
                return new Rational(0, left.Bottom * right.Bottom);
            }
            var newRational = new Rational(left.Top * right.Top, left.Bottom * right.Bottom);
            newRational.Simplify();
            return newRational;
        }

        public static Rational operator /(Rational left, Rational right)
        {
            var newRational = new Rational(left.Top * right.Bottom, left.Bottom * right.Top);
            newRational.Simplify();
            return newRational;
        }

        public static Rational operator +(Rational left, Rational right)
        {
            Rational res;
            if (left.Top == 0)
            {
                res = right;
            }
            else if (right.Top == 0)
            {
                res = left;
            }
            else if (right.Bottom == left.Bottom)
            {
                res = new Rational(left.Top + right.Top, left.Bottom);
            }
            else
            {
                res = new Rational(left.Top * right.Bottom + left.Bottom * right.Top, left.Bottom * right.Bottom);
            }
            res.Simplify();
            return res;
        }

        public static Rational operator -(Rational left, Rational right)
        {
            Rational res;
            if (left.Top == 0)
            {
                res = new Rational(-right.Top, right.Bottom);
            }
            else if (right.Top == 0)
            {
                res = left;
            }
            else if (right.Bottom == left.Bottom)
            {
                res = new Rational(left.Top - right.Top, left.Bottom);
            }
            else
            {
                res = new Rational(left.Top * right.Bottom - left.Bottom * right.Top, left.Bottom * right.Bottom);
            }
            res.Simplify();
            return res;
        }

        public static bool operator <=(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return left.Top <= right.Top;
            }
            else
            {
                return left.Top * right.Bottom - left.Bottom * right.Top <= 0;
            }
        }

        public static bool operator >=(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return left.Top >= right.Top;
            }
            else
            {
                return left.Bottom * right.Top - left.Top * right.Bottom >= 0;
            }
        }

        public static bool operator <(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return left.Top < right.Top;
            }
            else
            {
                return left.Top * right.Bottom - left.Bottom * right.Top < 0;
            }
        }



        public static bool operator >(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return left.Top > right.Top;
            }
            else
            {
                return left.Bottom * right.Top - left.Top * right.Bottom > 0;
            }
        }

        public void Simplify()
        {
            var gcd = BigInteger.GreatestCommonDivisor(this.Top, this.Bottom);
            if (gcd != 1)
            {
                this.Top /= gcd;
                this.Bottom /= gcd;
            }
        }
        #endregion
    }
}
