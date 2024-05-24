using System.Collections.Generic;
//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace Custom.Editor.DataTableTools
{
    public sealed class DataTableGeneratorUtil
    {
        private const string ATTRIBUTE_TYPE_CSV_FILE_NAME = "Assets/Plugins/SharedCore/Res/DataTable/{0}/Csv/EntityAttribute.csv";
        public static string ATTRIBUTE_TYPE_FILE_TEMPLATE_NAME = "Assets/Plugins/SharedCore/Src/Editor/DataTableGenerator/Template/AttributeTypeTemplate.txt";
        public static string ATTRIBUTE_TYPE_FILE_NAME = "Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/AttributeType.cs";
        public static string SVNCsvPath { get; private set; }
        public static string SVNConfigPath { get; private set; }

        public static void SetPath(string configPath,string suffix)
        {
            if (configPath is null)
            {
                return;
            }
            SVNConfigPath = configPath;
            SVNCsvPath = configPath + suffix;
        }

        public static void UpdateCsv()
        {
            DirectoryInfo direction = new(SVNCsvPath);
            //获取direction 下的所有文件夹
            DirectoryInfo[] allFolders = direction.GetDirectories();
            foreach (DirectoryInfo lanFolder in allFolders)
            {
                string lan = lanFolder.Name;
                string csvPath = DataTableGenerator.GetCsvFolderPath(lan);
                if (Directory.Exists(csvPath))
                {
                    Directory.Delete(csvPath, true);
                }
                _ = Directory.CreateDirectory(csvPath);
                FileInfo[] files = lanFolder.GetFiles("*.csv", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    string dest = Path.Combine(csvPath, files[i].Name);
                    string src = files[i].FullName;
                    File.Copy(src, dest, true);
                }
            }
        }

        public static void GenerateDataTables()
        {
            DirectoryInfo svnDirection = new(SVNCsvPath);
            //获取direction 下的所有文件夹
            DirectoryInfo[] allFolders = svnDirection.GetDirectories();

            foreach (DirectoryInfo item in allFolders)
            {
                string lan = item.Name;
                string tableBytesFolderPath = DataTableGenerator.GetByteFolderPath(lan);
                //清空bytes目录
                if (Directory.Exists(tableBytesFolderPath))
                {
                    Directory.Delete(tableBytesFolderPath, true);
                }
                _ = Directory.CreateDirectory(tableBytesFolderPath);

                string tableCsvFolderPath = DataTableGenerator.GetCsvFolderPath(lan);
                DirectoryInfo direction = new(tableCsvFolderPath);
                FileInfo[] files = direction.GetFiles("*.csv", SearchOption.AllDirectories);
                List<string> tableNames = new();
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = files[i].Name;
                    string extension = files[i].Extension;
                    string fullName = files[i].FullName;
                    string dataTableName = fileName[..^extension.Length];
                    tableNames.Add(dataTableName);
                    DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(fullName);
                    if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                    {
                        Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                        break;
                    }

                    DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName, lan);
                }
            }
        }

        public static void GenerateDataTableCodes()
        {
            //清空脚本目录
            if (Directory.Exists(DataTableGenerator.CSHARP_CODE_PATH))
            {
                Directory.Delete(DataTableGenerator.CSHARP_CODE_PATH, true);
            }
            _ = Directory.CreateDirectory(DataTableGenerator.CSHARP_CODE_PATH);

            string csvPath = DataTableGenerator.GetCsvFolderPath("En");//TODO:hard code
            DirectoryInfo direction = new(csvPath);
            FileInfo[] files = direction.GetFiles("*.csv", SearchOption.AllDirectories);
            List<string> tableNames = new();
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Name;
                string extension = files[i].Extension;
                string fullName = files[i].FullName;
                string dataTableName = fileName[..^extension.Length];
                tableNames.Add(dataTableName);
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(fullName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }
            DataTableGenerator.GenerateConfigFile(tableNames.ToArray());
        }

        public static string GetAttributeTypeCsvPath(string lan)
        {
            return string.Format(ATTRIBUTE_TYPE_CSV_FILE_NAME, lan);
        }

        public static void GenerateAttributeTypeFile()
        {
            DirectoryInfo svnDirection = new(SVNCsvPath);
            DirectoryInfo[] allFolders = svnDirection.GetDirectories();
            if (allFolders.Length == 0)
            {
                throw new Exception($"AttributeType csv file is not exist.");
            }
            string lan = allFolders[0].Name;//取第一个文件夹作为lan就行
            string path = GetAttributeTypeCsvPath(lan);
            if (!File.Exists(path))
            {
                throw new Exception($"AttributeType csv file '{path}' is not exist.");
            }

            try
            {
                string template = File.ReadAllText(ATTRIBUTE_TYPE_FILE_TEMPLATE_NAME, Encoding.UTF8);
                StringBuilder stringBuilder = new(template);
                _ = stringBuilder.Replace("__DATA_TABLE_NAMES__", GeneratorAttributeType(lan));

                using (FileStream fileStream = new(ATTRIBUTE_TYPE_FILE_NAME, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stream = new(fileStream, Encoding.UTF8))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log(Utility.Text.Format("Generate AttributeType file '{0}' success.", ATTRIBUTE_TYPE_FILE_NAME));
                return;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Generate AttributeType file '{0}' failure, exception is '{1}'.", ATTRIBUTE_TYPE_FILE_NAME, exception));
                return;
            }
        }
        public static string GeneratorAttributeType(string lan)
        {
            string csvPath = GetAttributeTypeCsvPath(lan);
            string tableText = File.ReadAllText(csvPath, Encoding.GetEncoding(TableDefine.DATA_TABLE_ENCODING));
            List<string[]> rawValues = CSVSerializer.ParseCSV(tableText);

            StringBuilder stringBuilder = new();
            int keyIndex = 0;
            int idIndex = 0;
            for (int i = 0; i < rawValues[0].Length; i++)
            {
                if (rawValues[0][i] == "name-string")
                {
                    keyIndex = i;
                }
                if (rawValues[0][i] == "id-int")
                {
                    idIndex = i;
                }
            }
            for (int i = TableDefine.DATA_TABLE_START_ROW; i < rawValues.Count; i++)
            {
                _ = stringBuilder
                    .AppendLine($"    {rawValues[i][keyIndex]} = {rawValues[i][idIndex]},");
            }

            return stringBuilder.ToString();
        }

    }
}
