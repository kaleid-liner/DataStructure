using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructure.HuffmanZip
{
    enum MagicNumber : int { Compressed = 114514, UnCompressed = 19260817 }

    public static class HuffmanZip
    {
        static public void Compress(string sourceFilePath, string destinationFilePath)
        {
            HuffmanEncoder encoder = new HuffmanEncoder();
            byte[] bytes = File.ReadAllBytes(sourceFilePath);
            encoder.Push(bytes);
            var code = encoder.EncodeAll();
            File.WriteAllBytes(destinationFilePath, code.ToArray());
        }


        static public void Decompress(string sourceFilePath, string destinationFilePath)
        {

            using (HuffmanDecoder decoder = new HuffmanDecoder(File.OpenRead(sourceFilePath)))
            {
                var code = decoder.DecodeAll();
                using (var binaryOutput = new BinaryWriter(File.OpenWrite(destinationFilePath)))
                {
                    binaryOutput.Write(code.ToArray());
                }
            }
        }

        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

    }
}
