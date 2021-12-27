using Algorithms.Data;
using System;
using System.Collections.Generic;
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
        private static byte GetCountOfBits(string bits,ref int currentPos)
        {
            var sb = new StringBuilder(6);
            for(int j = 0; j < 6; j++, currentPos++)
            {
                sb.Append(bits[currentPos]);
            }

            return (byte)(Convert.ToByte(sb.ToString(), fromBase: 2)+1);
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

        public static byte[] Encode(string text)
        {
            var data = CalculateNumeratorsAsync(text);
            return TransformNumeratorsToBinaryCode(data);
        }
        private  static byte[] TransformNumeratorsToBinaryCode(ulong[] numerators)
        {
            var bytes = new List<byte>();
            byte countOfBits;
            int currentByte = 0;
            int currentMask = 128;
            int j = 0;
            var sum = 0;

            //Учитывая что любое число типа ulong может быть представлено не более чем 64 битами
            //Мы можем представить число бит, необходимых для записи числителя с помощью 6 бит
            //6 бит позволяют закодировать от 0 до 63 символов
            //будем считать так, что 000000 = 1, 00001 = 2, ..., 111111 = 64
            string countOfBitsBuffer;



            foreach (var numerator in numerators)
            {
                countOfBits = (byte)((Math.Log(numerator, 2)+0.99)-1);
                sum += countOfBits+1+6;
                countOfBitsBuffer = Convert.ToString(countOfBits, toBase: 2);

                //Добавляем незначащие нули
                for(int i = 6 - countOfBitsBuffer.Length; i > 0; i--)
                {
                    countOfBitsBuffer = "0" + countOfBitsBuffer;
                }
                AddBitsToByte(ref currentByte, ref currentMask, countOfBitsBuffer, 6, bytes, ref j);

                countOfBitsBuffer = Convert.ToString(unchecked((long)numerator),toBase:2);
                AddBitsToByte(ref currentByte, ref currentMask, countOfBitsBuffer, (byte)(countOfBits+1), bytes, ref j);
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
                if(bits[i] == '1')
                {
                    currentByte |= currentMask;
                }
                currentMask >>= 1;
                j++;
                i++;
                if(j == 8)
                {
                    resultBytes.Add((byte)currentByte);
                    j = 0;
                    currentByte = 0;
                    currentMask = 128;
                }
            }
        }
        private  static ulong[] CalculateNumeratorsAsync(string text)
        {
            var strings = SplitBigStringToStringsWith16Length(text);

            var numerators = strings.Select(x => Encode16Symbols(x)).ToArray();
            //await Task.WhenAll(numerators);
            return numerators;
        }
        private static string[] SplitBigStringToStringsWith16Length(string text)
        {
            string[] strings = null;
            if (text.Length % 16 == 0)
            {
                strings = new string[text.Length / 16];
            }
            else
            {
                strings = new string[(text.Length / 16)+1];
            }

            var sb = new StringBuilder(16);

            for (int i = 0; i < text.Length / 16; i++)
            {
                for(int j = 0; j < 16; j++)
                {
                    sb.Append(text[i*16+j]);
                }
                strings[i] = sb.ToString();
                sb.Clear();
            }

            var ostatok = text.Length % 16;
            if(ostatok != 0)
            {
                for (int i = 16 * (text.Length / 16); i < text.Length; i++)
                {
                    sb.Append(text[i]);
                }
                strings[text.Length / 16] = sb.ToString();
            }

            return strings;
        }
        private static ulong Encode16Symbols(string text)
        {
            if (text.Length <= 16)
            {
                var intervals = DescribeIntervals(text);
                unchecked
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
                    return oldLeft.Top;
                }
            }
            throw new ArgumentException("Максимальное число элементов, которое можно закодировать за раз 16");
        }

        private static Dictionary<Char, Symbol> DescribeIntervals(string text)
        {
            var symbols = new Dictionary<Char, Symbol>();
            foreach (char c in text)
            {
                if (!symbols.ContainsKey(c))
                {
                    symbols.Add(c, new Symbol(c,1));
                }
                else
                {
                    symbols[c].Frequency = symbols[c].Frequency + 1;
                }
            }
            ulong prevTop = 0;
            foreach(var value in symbols.Values)
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
        public ulong Top { get; set; }
        public ulong Bottom { get; set; }

        public Rational(ulong top, ulong bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public static Rational operator *(Rational left, Rational right)
        {
            unchecked
            {
                if (left.Top == 0 || right.Top == 0)
                {
                    return new Rational(0, left.Bottom * right.Bottom);
                }
                return new Rational(left.Top * right.Top, left.Bottom * right.Bottom);
            }
        }

        public static Rational operator +(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return new Rational(left.Top + right.Top, left.Bottom);
            }
            if (right.Bottom == 0)
            {
                var tmp = (ulong.MaxValue - 1) / 2 + 1;
                left.Bottom = left.Bottom / 2;
                tmp = tmp / left.Bottom;
                return new Rational(left.Top * tmp, 0);
            }
            else if (left.Bottom > right.Bottom)
            {
                var tmp = left.Bottom / right.Bottom;
                return new Rational(left.Top + right.Top * tmp, left.Bottom);
            }
            else
            {
                var tmp = right.Bottom / left.Bottom;
                return new Rational(left.Top * tmp + right.Top, right.Bottom);
            }
        }

        public static Rational operator -(Rational left, Rational right)
        {
            if (left.Bottom == right.Bottom)
            {
                return new Rational(left.Top - right.Top, left.Bottom);
            }
            if (left.Bottom > right.Bottom)
            {
                var tmp = left.Bottom / right.Bottom;
                return new Rational(left.Top - right.Top * tmp, left.Bottom);
            }
            else
            {
                var tmp = right.Bottom / left.Bottom;
                return new Rational(left.Top - tmp + right.Top, right.Bottom);
            }
        }
    }


}
