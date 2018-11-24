using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DataStructure.HuffmanZip
{
    class HuffmanEncoder
    {
        private uint[] frequency;
        private HuffmanTree codeTree;
        private List<byte> remainingBytes;

        public HuffmanEncoder()
        {
            frequency = new uint[256];
            for (int i = 0; i < frequency.Length; i++)
                frequency[i] = 0;
            remainingBytes = new List<byte>();
        }

        public void Push(byte[] input)
        {
            foreach (byte b in input)
            {
                Push(b);
            }
        }

        public void Push(byte input)
        {
            frequency[input] += 1;
            remainingBytes.Add(input);
        }

        public void ClearState()
        {
            for (int i = 0; i < frequency.Length; i++)
            {
                frequency[i] = 0;
            }
            remainingBytes.Clear();
        }

        public IEnumerable<byte> EncodeAll()
        {
            MagicNumber finalNumber;
            List<byte> finalCode;
            codeTree = new HuffmanTree(frequency);

            List<byte> code = new List<byte>(new byte[5]);//place reserved for MagicNumber and padding
            foreach (uint i in frequency)
            {
                byte[] freq = BitConverter.GetBytes(i);
                code.AddRange(freq);
            }
            byte bitBuffer = 0;
            int usedBit = 0;
            foreach (byte currentByte in remainingBytes)
            {
                string bcode = codeTree[currentByte];
                foreach (char c in bcode)
                {
                    if (c == '1')
                    {
                        bitBuffer = (byte)(bitBuffer | (1 << usedBit));
                    }
                    usedBit++;
                    if (usedBit == 8)
                    {
                        code.Add(bitBuffer);
                        usedBit = 0;
                        bitBuffer = 0;
                    }
                }
            }
            if (usedBit != 0)
            {
                code.Add(bitBuffer);
                code[4] = (byte)(8 - usedBit);
            }
            else code[4] = 0;
            if (code.Count < remainingBytes.Count)
            {
                finalNumber = MagicNumber.Compressed;
                finalCode = code;
            }
            else
            {
                finalNumber = MagicNumber.UnCompressed;
                finalCode = new List<byte>() { 4, 4, 4, 4 };
                finalCode.AddRange(remainingBytes);
            }
            byte[] magicNumber = BitConverter.GetBytes((int)finalNumber);
            finalCode[0] = magicNumber[0];
            finalCode[1] = magicNumber[1];
            finalCode[2] = magicNumber[2];
            finalCode[3] = magicNumber[3];
            return finalCode;
        }

        public string EncodeByte(byte input)
        {
            if (codeTree == null)
                codeTree = new HuffmanTree(frequency);
            return codeTree[input];
        }

        public void EndInput()
        {
            codeTree = new HuffmanTree(frequency);
        }
    }
}
