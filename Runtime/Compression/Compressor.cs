using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CustomTools.Compression
{
    public static class Compressor
    {
        public static string Compress(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();

            using (GZipStream stream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                stream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            byte[] compressed = new byte[memoryStream.Length];
            memoryStream.Read(compressed, 0, compressed.Length);

            byte[] gZipBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);

            return Convert.ToBase64String(gZipBuffer);
        }

        public static string Decompress(this string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);

            using (MemoryStream stream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                stream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                byte[] buffer = new byte[dataLength];
                stream.Position = 0;

                using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}