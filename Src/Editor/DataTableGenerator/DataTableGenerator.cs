//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Custom.Editor.DataTableTools
{
    public sealed class DataTableGenerator
    {
        private const string DATA_TABLE_BYTE_PATH = "Assets/Plugins/SharedCore/Res/DataTable/{0}/Bytes";
        private const string DATA_TABLE_CSV_PATH = "Assets/Plugins/SharedCore/Res/DataTable/{0}/Csv";
        public const string CSHARP_CODE_PATH = "Assets/Plugins/SharedCore/Src/Runtime/Csv/TableRow";
        private const string CSHARP_CODE_TEMPLATE_FILE_NAME = "Assets/Plugins/SharedCore/Src/Editor/DataTableGenerator/Template/DataTableCodeTemplate.txt";
        public static string DATA_TABLE_CONFIG_TEMPLATE_NAME = "Assets/Plugins/SharedCore/Src/Editor/DataTableGenerator/Template/DataTableConfigTemplate.txt";
        public static string DATA_TABLE_CONFIG_NAME = "Assets/Plugins/SharedCore/Src/Runtime/Csv/DataTableConfig.cs";

        public static DataTableProcessor CreateDataTableProcessor(string fullName)
        {
            return new DataTableProcessor(fullName, Encoding.GetEncoding(TableDefine.DATA_TABLE_ENCODING), 1, 2, null, 0, 3);
        }

        public static bool CheckRawData(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            if (dataTableProcessor is null)
            {
                throw new ArgumentNullException(nameof(dataTableProcessor));
            }

            if (string.IsNullOrEmpty(dataTableName))
            {
                return false;
            }

            return true;
        }

        public static string GetCodePath(string dataTableName)
        {
            return Utility.Path.GetRegularPath(Path.Combine(CSHARP_CODE_PATH, "DR" + dataTableName + ".cs"));
        }

        public static string GetByteFolderPath(string lan)
        {
            return Utility.Path.GetRegularPath(Path.Combine(string.Format(DATA_TABLE_BYTE_PATH, lan)));
        }

        public static string GetBytePath(string dataTableName, string lan)
        {
            return Utility.Path.GetRegularPath(Path.Combine(GetByteFolderPath(lan), dataTableName + ".bytes"));
        }

        public static string GetCsvFolderPath(string lan)
        {
            return Utility.Path.GetRegularPath(Path.Combine(string.Format(DATA_TABLE_CSV_PATH, lan)));
        }

        public static string GetCsvPath(string dataTableName, string lan)
        {
            return Utility.Path.GetRegularPath(Path.Combine(GetCsvFolderPath(lan), dataTableName + ".csv"));
        }

        public static void GenerateDataFile(DataTableProcessor dataTableProcessor, string dataTableName, string lan)
        {
            string binaryDataFileName = GetBytePath(dataTableName, lan);
            if (!dataTableProcessor.GenerateDataFile(binaryDataFileName) && File.Exists(binaryDataFileName))
            {
                File.Delete(binaryDataFileName);
            }
        }

        public static void GenerateCodeFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            _ = dataTableProcessor.SetCodeTemplate(CSHARP_CODE_TEMPLATE_FILE_NAME, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            string csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(CSHARP_CODE_PATH, "DR" + dataTableName + ".cs"));
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;
            _ = codeContent.Replace("__DATA_TABLE_CLASS_NAME__", "DR" + dataTableName);
            _ = codeContent.Replace("__DATA_TABLE_ID_COMMENT__", "获取" + dataTableProcessor.GetComment(dataTableProcessor.IdColumn) + "。");
            _ = codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
            _ = codeContent.Replace("__DATA_TABLE_PARSER__", GenerateDataTableParser(dataTableProcessor));
        }

        private static string GenerateDataTableProperties(DataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new();
            bool firstProperty = true;
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    continue;
                }

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    _ = stringBuilder.AppendLine().AppendLine();
                }

                _ = stringBuilder
                    .AppendLine("    /// <summary>")
                    .AppendFormat("  /**获取{0}。*/", dataTableProcessor.GetComment(i)).AppendLine()
                    .AppendLine("    /// </summary>")
                    .AppendFormat("    public {0} {1}", dataTableProcessor.GetLanguageKeyword(i), dataTableProcessor.GetFirstLetterToUpperName(i)).AppendLine()
                    .AppendLine("    {")
                    .AppendLine("        get;")
                    .AppendLine("        private set;")
                    .Append("    }");
            }

            return stringBuilder.ToString();
        }

        private static string GenerateDataTableParser(DataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new();
            _ = stringBuilder
                .AppendLine("    public override bool ParseDataRow(string dataRowString, object userData)")
                .AppendLine("    {")
                .AppendLine("        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);")
                .AppendLine()
                .AppendLine("        int index = 0;");

            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    _ = stringBuilder.AppendLine("        index++;");
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    _ = stringBuilder.AppendLine("        _id = int.Parse(columnStrings[index++]);");
                    continue;
                }

                if (dataTableProcessor.GetExtensionParseKey(i) == null)
                {
                    string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                    if (languageKeyword == "string")
                    {
                        _ = stringBuilder.AppendFormat("        {0} = columnStrings[index++];", dataTableProcessor.GetFirstLetterToUpperName(i)).AppendLine();
                    }
                    else
                    {
                        _ = stringBuilder.AppendFormat("        {0} = {1}.Parse(columnStrings[index++]);", dataTableProcessor.GetFirstLetterToUpperName(i), languageKeyword).AppendLine();
                    }
                }
                else
                {
                    _ = stringBuilder.AppendFormat("        {0} = DataTableParseUtil.{1}(columnStrings[index++]);", dataTableProcessor.GetFirstLetterToUpperName(i), dataTableProcessor.GetExtensionParseKey(i)).AppendLine();
                }
            }

            _ = stringBuilder.AppendLine()
                .AppendLine("        return true;")
                .AppendLine("    }");

            _ = stringBuilder.AppendLine()
                .AppendLine()
                .AppendLine("    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)")
                .AppendLine("    {")
                .AppendLine("        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))")
                .AppendLine("        {")
                .AppendLine("            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))")
                .AppendLine("            {");

            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    _ = stringBuilder.AppendLine("                _id = binaryReader.Read7BitEncodedInt32();");
                    continue;
                }

                string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                if (languageKeyword.Equals("int")
                    || languageKeyword.Equals("uint")
                    || languageKeyword.Equals("long")
                    || languageKeyword.Equals("ulong"))
                {
                    _ = stringBuilder.AppendFormat("                {0} = binaryReader.Read7BitEncoded{1}();", dataTableProcessor.GetFirstLetterToUpperName(i), dataTableProcessor.GetType(i).Name).AppendLine();

                }
                else if (languageKeyword.Equals("string[]")
                    || languageKeyword.Equals("bool[]")
                    || languageKeyword.Equals("int[]")
                    || languageKeyword.Equals("double[]"))
                {
                    _ = stringBuilder.AppendFormat("                {0} = binaryReader.ReadArray<{1}>();", dataTableProcessor.GetFirstLetterToUpperName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
                else if (languageKeyword.Equals("string[][]")
                    || languageKeyword.Equals("bool[][]")
                    || languageKeyword.Equals("int[][]")
                    || languageKeyword.Equals("double[][]"))
                {
                    _ = stringBuilder.AppendFormat("                {0} = binaryReader.ReadArrayList<{1}>();", dataTableProcessor.GetFirstLetterToUpperName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
                else
                {
                    _ = stringBuilder.AppendFormat("                {0} = binaryReader.Read{1}();", dataTableProcessor.GetFirstLetterToUpperName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
            }

            _ = stringBuilder
                .AppendLine("            }")
                    .AppendLine("        }")
                    .AppendLine()
                    .AppendLine("        return true;")
                    .Append("    }");

            return stringBuilder.ToString();
        }

        public static void GenerateConfigFile(string[] tableNames)
        {
            string template = File.ReadAllText(DATA_TABLE_CONFIG_TEMPLATE_NAME, Encoding.UTF8);
            StringBuilder stringBuilder = new(template);
            _ = stringBuilder.Replace("__DATA_TABLE_NAMES__", GeneratorTableNames(tableNames));

            try
            {
                using (FileStream fileStream = new(DATA_TABLE_CONFIG_NAME, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stream = new(fileStream, Encoding.UTF8))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log(Utility.Text.Format("Generate code file '{0}' success.", DATA_TABLE_CONFIG_NAME));
                return;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Generate code file '{0}' failure, exception is '{1}'.", DATA_TABLE_CONFIG_NAME, exception));
                return;
            }
        }

        private static string GeneratorTableNames(string[] tableNames)
        {
            StringBuilder stringBuilder = new();
            _ = stringBuilder
                .AppendLine("    public static readonly string[] DataTableNames =")
                .AppendLine("    {");
            for (int i = 0; i < tableNames.Length; i++)
            {
                string name = tableNames[i];
                _ = stringBuilder.AppendLine($"        \"{name}\",");
            }

            _ = stringBuilder.AppendLine("    };");
            return stringBuilder.ToString();
        }

    }
}
