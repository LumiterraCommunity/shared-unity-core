using System.IO;

public static class FileTool
{
    /// <summary>
    /// 读取文件内容
    /// </summary>
    /// <param name="path">要读取的文件路径</param>
    /// <param name="encoding">编码格式</param>
    /// <returns>返回文件内容</returns>
    public static string ReadFileText(string path, System.Text.Encoding encoding)
    {
        string result;
        if (!File.Exists(path))
        {
            result = "";
        }
        else
        {
            FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new(stream, encoding);
            result = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 确保一定有文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void EnsureDir(string path)
    {
        FileInfo fi = new(path);
        if (!Directory.Exists(fi.Directory.FullName))
        {
            _ = Directory.CreateDirectory(fi.Directory.FullName);
        }
    }

    /// <summary>
    /// 向指定文件写入内容
    /// </summary>
    /// <param name="path">要写入内容的文件完整路径</param>
    /// <param name="content">要写入的内容</param>
    /// <param name="encoding">编码格式</param>
    public static void WriteFile(string path, string content, System.Text.Encoding encoding)
    {
        object obj = new();
        EnsureDir(path);
        if (!File.Exists(path))
        {
            FileStream fileStream = File.Create(path);
            fileStream.Close();
        }
        lock (obj)
        {
            using StreamWriter streamWriter = new(path, false, encoding);
            streamWriter.WriteLine(content);
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }

}