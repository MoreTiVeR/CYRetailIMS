using System.IO.Compression;
using System.Text;

namespace CYRetailIMS.Application.Common.Extensions;
public static class CompressorHelperIOS
{
    public static string Compress(this string text)
    {
        var buffer = Encoding.UTF8.GetBytes(text);
        var memoryStream = new MemoryStream();
        using (var stream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            stream.Write(buffer, 0, buffer.Length);
        }
        memoryStream.Position = 0;
        var compressed = new byte[memoryStream.Length];
        memoryStream.Read(compressed, 0, compressed.Length);
        var gZipBuffer = new byte[compressed.Length + 4];
        Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    [Obsolete("Move to Decompress")]
    public static string DecompressNET5(this string compressedText)
    {
        var gZipBuffer = Convert.FromBase64String(compressedText);
        using (var memoryStream = new MemoryStream())
        {
            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
            var buffer = new byte[dataLength];
            memoryStream.Position = 0;
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }

    public static string Decompress(this string compressedText)
    {
        var gZipBuffer = Convert.FromBase64String(compressedText);

        using var memoryStream = new MemoryStream();
        int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
        memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

        var buffer = new byte[dataLength];
        memoryStream.Position = 0;

        using var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);

        int totalRead = 0;
        while (totalRead < buffer.Length)
        {
            int bytesRead = gZipStream.Read(buffer, totalRead, buffer.Length - totalRead);
            if (bytesRead == 0) break;
            totalRead += bytesRead;
        }

        return Encoding.UTF8.GetString(buffer);
    }
}
