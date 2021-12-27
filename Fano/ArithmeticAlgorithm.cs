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
        public static string Decode(byte[] data)
        {
            var bits = BytesToString(data);
            List<ulong> numerators = new List<ulong>();

            int i = 0;
            byte countOfBits;
            while (true)
            {
                countOfBits = GetCountOfBits(bits, ref i);
                if (countOfBits == 1) break;
                numerators.Add(GetNumerator(bits, ref i, countOfBits));
                if (bits.Length - i < 6) break;
            }

            return "adf";

        }



        #region useless

        //private static string ConvertNumeratorToString()
        //{

        //}

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
            var rational = GetRational(text, intervals);
            using (var bw = new BinaryWriter(File.Open(outputPath,FileMode.Create)))
            {
                var values = intervals.Values.ToArray();
                bw.Write((byte)values.Length);
                for(int i = 0; i < values.Length; i++)
                {
                    bw.Write(values[i].Value);
                    bw.Write((short)values[i].Frequency);
                }
                var numeratorBytes = rational.Top.ToByteArray();
                bw.Write((short)numeratorBytes.Length);
                bw.Write(numeratorBytes);
                var denumeratorBytes = rational.Bottom.ToByteArray();
                bw.Write((short)denumeratorBytes.Length);
                bw.Write(denumeratorBytes);
            }
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
        private static Rational GetRational(string text, Dictionary<Char,Symbol> intervals)
        {
            Rational oldLeft = new Rational(0, (ulong)text.Length);
            Rational oldRight = new Rational((ulong)text.Length, (ulong)text.Length);
            Rational newLeft;
            Rational newRight;
            foreach (var symbol in text)
            {
                newRight = oldLeft + (oldRight - oldLeft) * intervals[symbol].Right;
                newLeft = oldLeft + (oldRight - oldLeft) * intervals[symbol].Left;
                oldRight = newRight;
                oldLeft = newLeft;
            }
            return oldLeft;

        }
        private static string GetText(int count, Rational rational, Symbol[] symbols)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {

                for(int j = 0; j < symbols.Length; j++)
                {
                    if(symbols[j].Left<= rational &&  rational<symbols[j].Right)
                    {
                        sb.Append(symbols[j].Value);
                        rational = (rational - symbols[j].Left)/(symbols[j].Right - symbols[j].Left);
                        break;
                    }
                }
            }
            return sb.ToString();
        }

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
            ulong prevTop = 0;
            foreach (var value in symbols.Values)
            {
                value.Left = new Rational(prevTop, (ulong)text.Length);
                prevTop += value.Frequency;
                value.Right = new Rational(prevTop, (ulong)text.Length);
            }
            return symbols;
        }

    }

    public class Symbol
    {
        private Char value;
        public Char Value => value;
        public Rational Left { get; set; }
        public Rational Right { get; set; }

        public ulong Frequency { get; set; }

        public Symbol(Char value, Rational left, Rational right, ulong frequency)
        {
            this.value = value;
            this.Left = left;
            this.Right = right;
            this.Frequency = frequency;
        }

        public Symbol(Char value, ulong frequency)
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
