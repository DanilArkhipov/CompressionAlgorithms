using Algorithms.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public static class HammingAlgorithm
    {
        //Проверочная матрица
        private readonly static char[][] checkMatrix =
        {
            new char[] { '1','0','1','0','1','0','1' },
            new char[] { '0','1','1','0','0','1','1' },
            new char[] { '0','0','0','1','1','1','1' }
        };

        public static string Encode(string inputCode)
        {
            if (inputCode.Length != 4) throw new ArgumentException("Длина кода должна быть равна 4");
            else
            {
                char[] newCode = { '0', '0', '0', '0', '0', '0', '0' };

                newCode[2] = inputCode[0];
                newCode[4] = inputCode[1];
                newCode[5] = inputCode[2];
                newCode[6] = inputCode[3];

                char[] newCodeWithParityBits = new char[7];
                newCode.CopyTo(newCodeWithParityBits, 0);

                newCodeWithParityBits[0] = CountParityBit(newCode, checkMatrix[0]);
                newCodeWithParityBits[1] = CountParityBit(newCode, checkMatrix[1]);
                newCodeWithParityBits[3] = CountParityBit(newCode, checkMatrix[2]);

                return new string(newCodeWithParityBits);
            }
        }

        public static string Decode(string inputCode)
        {
            if (inputCode.Length != 7) throw new ArgumentException("Длина кода должна быть равна 7");
            else
            {
                char[] code = inputCode.ToCharArray();

                char[] errorCode = new char[3];
                errorCode[0] = CountParityBit(code, checkMatrix[0]);
                errorCode[1] = CountParityBit(code, checkMatrix[1]);
                errorCode[2] = CountParityBit(code, checkMatrix[2]);

                int errorIndex = CodeToInt(errorCode);

                if (errorIndex != 0)
                {
                    if (code[errorIndex - 1] == '0') code[errorIndex - 1] = '1';
                    else
                    {
                        code[errorIndex - 1] = '0';
                    }
                }

                char[] decodedCode = new char[4];
                decodedCode[0] = code[2];
                decodedCode[1] = code[4];
                decodedCode[2] = code[5];
                decodedCode[3] = code[6];

                return new string(decodedCode);
            }
        }

        private static char CountParityBit(char[] code, char[] matrixString)
        {
            bool isEven = true;

            for (int i = 0; i < matrixString.Length; i++)
            {
                if (code[i] == '1' && matrixString[i] == '1') isEven = !isEven;
            }

            return isEven ? '0' : '1';
        }

        private static int CodeToInt(char[] code)
        {
            int number = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '1')
                {
                    number += (int)Math.Pow(2, i);
                }
            }
            return number;
        }

    }
}
