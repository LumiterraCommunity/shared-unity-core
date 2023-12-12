using System.IO;
using System.IO.Compression;
using System.Text;

/// <summary>
/// 压缩工具
/// </summary>
public static class CompressionUtil
{
    /// <summary>
    /// 压缩一个字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="type">压缩类型</param>
    /// <returns></returns>
    public static byte[] CompressString(string input, eCompressType type)
    {
        return CompressString(input, type, Encoding.UTF8);
    }
    /// <summary>
    ///  压缩一个字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="type">压缩类型</param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static byte[] CompressString(string input, eCompressType type, Encoding encoding)
    {
        encoding ??= Encoding.UTF8;

        byte[] inputBytes = encoding.GetBytes(input);

        using MemoryStream outputStream = new();

        using (Stream compressStream = type == eCompressType.GZip
        ? new GZipStream(outputStream, CompressionMode.Compress)
        : new DeflateStream(outputStream, CompressionMode.Compress))
        {
            compressStream.Write(inputBytes, 0, inputBytes.Length);
        }

        return outputStream.ToArray();
    }

    /// <summary>
    /// 解压缩一个字符串
    /// </summary>
    /// <param name="compressedData"></param>
    /// <param name="type">压缩类型</param>
    /// <returns></returns>
    public static string DecompressString(byte[] compressedData, eCompressType type)
    {
        return DecompressString(compressedData, type, Encoding.UTF8);
    }
    /// <summary>
    /// 解压缩一个字符串
    /// </summary>
    /// <param name="compressedData"></param>
    /// <param name="type">压缩类型</param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string DecompressString(byte[] compressedData, eCompressType type, Encoding encoding)
    {
        encoding ??= Encoding.UTF8;

        using MemoryStream inputStream = new(compressedData);

        using Stream decompressStream = type == eCompressType.GZip
        ? new GZipStream(inputStream, CompressionMode.Decompress)
        : new DeflateStream(inputStream, CompressionMode.Decompress);

        using StreamReader reader = new(decompressStream, encoding);
        return reader.ReadToEnd();
    }
}

public enum eCompressType
{
    Deflate,//压缩低且快
    GZip,//压缩高且慢
}