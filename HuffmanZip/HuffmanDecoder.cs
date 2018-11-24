using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructure.HuffmanZip
{
    class HuffmanDecoder : IDisposable
    {
        private uint[] frequency;
        private HuffmanTree codeTree;
        private MagicNumber magic;
        private byte padding;
        private bool disposed = false;
        private BinaryReader reader;

        public HuffmanDecoder(Stream input)
        {
            frequency = new uint[256];
            ResetInput(input);
        }

        private void DecodeHeader()
        {
            int magicNumber = reader.ReadInt32();
            if (!Enum.IsDefined(typeof(MagicNumber), magicNumber))
                throw new FormatException("not a Huffman byte stream");
            magic = (MagicNumber)magicNumber;
            if (magic == MagicNumber.Compressed)
            {
                padding = reader.ReadByte();
                for (int i = 0; i < frequency.Length; i++)
                    frequency[i] = reader.ReadUInt32();
                BuildHuffmanTree();
            }
        }

        public void ResetInput(Stream input)
        {
            reader = new BinaryReader(input);
            DecodeHeader();
        }

        public IEnumerable<byte> DecodeAll()
        {
            byte[] bytes = reader.ReadAllBytes();
            return DecodeBytes(bytes);
        }

        public IEnumerable<byte> DecodeBytes(byte[] bytes)
        {
            if (magic == MagicNumber.UnCompressed)
                return bytes;
            List<byte> origBytes = new List<byte>();
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                byte b = bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    int orig = codeTree.Next((byte)(b & 1));
                    if (orig != -1)
                        origBytes.Add((byte)orig);
                    b >>= 1;
                }
            }
            byte rear = bytes.Last();
            for (int j = 0; j < 8 - padding; j++)
            {
                int orig = codeTree.Next((byte)(rear & 1));
                if (orig != -1)
                    origBytes.Add((byte)orig);
                rear >>= 1;
            }
            return origBytes;
        }

        private void BuildHuffmanTree()
        {
            codeTree = new HuffmanTree(frequency);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                reader.Dispose();
            }
            disposed = true;
        }
    }
}
