//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Custom.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private const string COMMENT_LINE_SEPARATOR = "#";
        private readonly string[] _nameRow;   //名字行
        private readonly string[] _typeRow;   //类型行
        private readonly string[] _defaultValueRow; //默认行
        private readonly string[] _commentRow;   //注释行

        private readonly DataProcessor[] _dataProcessor; //数据处理器
        private readonly string[][] _rawValues;
        public int RawRowCount => _rawValues.Length;
        public int RawColumnCount => _rawValues.Length > 0 ? _rawValues[0].Length : 0;
        public int ContentStartRow { get; }

        public int IdColumn { get; }

        private string _codeTemplate; //代码模板
        private DataTableCodeGenerator _codeGenerator; //代码处理器

        public DataTableProcessor(string dataTableFileName, Encoding encoding, int nameRow, int typeRow, int? defaultValueRow, int? commentRow, int contentStartRow)
        {
            if (string.IsNullOrEmpty(dataTableFileName))
            {
                throw new GameFrameworkException("Data table file name is invalid.");
            }

            if (!dataTableFileName.EndsWith(".csv", StringComparison.Ordinal))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data table file '{0}' is not a csv.", dataTableFileName));
            }

            if (!File.Exists(dataTableFileName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data table file '{0}' is not exist.", dataTableFileName));
            }

            UnityEngine.Debug.Log($"DataTableProcessor dataTableFileName={dataTableFileName}");

            try
            {
                string tableText = File.ReadAllText(dataTableFileName, encoding);
                List<string[]> rawValues = CSVSerializer.ParseCSV(tableText);
                string[] nameRawRow = new string[rawValues[0].Length];
                string[] typeRawRow = new string[rawValues[0].Length];
                for (int i = 0; i < rawValues[0].Length; i++)
                {
                    string nameTypeStr = rawValues[0][i];
                    string[] valueList = nameTypeStr.Split('-');
                    nameRawRow[i] = valueList[0];
                    typeRawRow[i] = valueList[1];
                }
                rawValues.Insert(1, nameRawRow);
                rawValues.Insert(2, typeRawRow);
                _rawValues = rawValues.ToArray();
            }
            catch (Exception e)
            {
                throw new GameFrameworkException(Utility.Text.Format("Parse data table file '{0}' failure.", dataTableFileName), e);
            }

            int rawRowCount = _rawValues.Length;
            int rawColumnCount = _rawValues[0].Length;

            if (nameRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name row '{0}' is invalid.", nameRow));
            }

            if (typeRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Type row '{0}' is invalid.", typeRow));
            }

            if (contentStartRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Content start row '{0}' is invalid.", contentStartRow));
            }


            if (nameRow >= rawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name row '{0}' >= raw row count '{1}' is not allow.", nameRow, rawRowCount));
            }

            if (typeRow >= rawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Type row '{0}' >= raw row count '{1}' is not allow.", typeRow, rawRowCount));
            }

            if (defaultValueRow.HasValue && defaultValueRow.Value >= rawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Default value row '{0}' >= raw row count '{1}' is not allow.", defaultValueRow.Value, rawRowCount));
            }

            if (commentRow.HasValue && commentRow.Value >= rawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Comment row '{0}' >= raw row count '{1}' is not allow.", commentRow.Value, rawRowCount));
            }

            if (contentStartRow > rawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Content start row '{0}' > raw row count '{1}' is not allow.", contentStartRow, rawRowCount));
            }


            _nameRow = _rawValues[nameRow];
            _typeRow = _rawValues[typeRow];
            _defaultValueRow = defaultValueRow.HasValue ? _rawValues[defaultValueRow.Value] : null;
            _commentRow = commentRow.HasValue ? _rawValues[commentRow.Value] : null;
            ContentStartRow = contentStartRow;

            //初始化每列数据处理器
            _dataProcessor = new DataProcessor[rawColumnCount];
            for (int i = 0; i < rawColumnCount; i++)
            {
                if (_nameRow[i] == "id")
                {
                    IdColumn = i;
                    _dataProcessor[i] = DataProcessorUtility.GetDataProcessor("id");
                }
                else
                {
                    _dataProcessor[i] = DataProcessorUtility.GetDataProcessor(_typeRow[i]);
                }
            }

            _codeTemplate = null;
            _codeGenerator = null;
        }

        public bool IsIdColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].IsId;
        }

        public bool IsCommentRow(int rawRow)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow));
            }

            return GetValue(rawRow, 0).StartsWith(COMMENT_LINE_SEPARATOR, StringComparison.Ordinal);
        }

        public bool IsCommentColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return string.IsNullOrEmpty(GetName(rawColumn)) || _dataProcessor[rawColumn].IsComment;
        }

        public string GetName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            if (IsIdColumn(rawColumn))
            {
                return "Id";
            }

            return _nameRow[rawColumn];
        }
        public string GetFirstLetterToUpperName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            if (IsIdColumn(rawColumn))
            {
                return "Id";
            }
            string name = _nameRow[rawColumn];
            if (name == null)
            {
                return null;
            }

            if (name.Length > 1)
            {
                return char.ToUpper(name[0]) + name[1..];
            }

            return name.ToUpper();
        }
        public bool IsSystem(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].IsSystem;
        }

        public Type GetType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].Type;
        }

        public string GetLanguageKeyword(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].LanguageKeyword;
        }

        public string GetExtensionParseKey(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].ExtensionParseKey;
        }

        public string GetDefaultValue(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _defaultValueRow?[rawColumn];
        }

        public string GetComment(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _commentRow?[rawColumn];
        }

        public string GetValue(int rawRow, int rawColumn)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow));
            }

            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }
            return _rawValues[rawRow][rawColumn];
        }

        public bool GenerateDataFile(string outputFileName)
        {
            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameFrameworkException("Output file name is invalid.");
            }

            try
            {
                using (FileStream fileStream = new(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter binaryWriter = new(fileStream, Encoding.UTF8))
                    {
                        for (int rawRow = ContentStartRow; rawRow < RawRowCount; rawRow++)
                        {
                            if (IsCommentRow(rawRow))
                            {
                                continue;
                            }

                            byte[] bytes = GetRowBytes(outputFileName, rawRow);
                            binaryWriter.Write7BitEncodedInt32(bytes.Length);
                            binaryWriter.Write(bytes);
                        }
                    }
                }

                Debug.Log(Utility.Text.Format("Parse data table '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Parse data table '{0}' failure, exception is '{1}'.", outputFileName, exception));
                return false;
            }
        }

        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                _codeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
                Debug.Log(Utility.Text.Format("Set code template '{0}' success.", codeTemplateFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Set code template '{0}' failure, exception is '{1}'.", codeTemplateFileName, exception));
                return false;
            }
        }

        public void SetCodeGenerator(DataTableCodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData = null)
        {
            if (string.IsNullOrEmpty(_codeTemplate))
            {
                throw new GameFrameworkException("You must set code template first.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameFrameworkException("Output file name is invalid.");
            }

            try
            {
                StringBuilder stringBuilder = new(_codeTemplate);
                _codeGenerator?.Invoke(this, stringBuilder, userData);

                using (FileStream fileStream = new(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stream = new(fileStream, encoding))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log(Utility.Text.Format("Generate code file '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Generate code file '{0}' failure, exception is '{1}'.", outputFileName, exception));
                return false;
            }
        }

        private byte[] GetRowBytes(string outputFileName, int rawRow)
        {
            using (MemoryStream memoryStream = new())
            {
                using (BinaryWriter binaryWriter = new(memoryStream, Encoding.UTF8))
                {
                    for (int rawColumn = 0; rawColumn < RawColumnCount; rawColumn++)
                    {
                        if (IsCommentColumn(rawColumn))
                        {
                            continue;
                        }

                        try
                        {
                            _dataProcessor[rawColumn].WriteToStream(this, binaryWriter, GetValue(rawRow, rawColumn));
                        }
                        catch
                        {
                            if (_dataProcessor[rawColumn].IsId || string.IsNullOrEmpty(GetDefaultValue(rawColumn)))
                            {
                                Debug.LogError(Utility.Text.Format("Parse raw value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, GetName(rawColumn), GetLanguageKeyword(rawColumn), GetValue(rawRow, rawColumn)));
                                return null;
                            }
                            else
                            {
                                Debug.LogWarning(Utility.Text.Format("Parse raw value failure, will try default value. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, GetName(rawColumn), GetLanguageKeyword(rawColumn), GetValue(rawRow, rawColumn)));
                                try
                                {
                                    _dataProcessor[rawColumn].WriteToStream(this, binaryWriter, GetDefaultValue(rawColumn));
                                }
                                catch
                                {
                                    Debug.LogError(Utility.Text.Format("Parse default value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, GetName(rawColumn), GetLanguageKeyword(rawColumn), GetComment(rawColumn)));
                                    return null;
                                }
                            }
                        }
                    }

                    return memoryStream.ToArray();
                }
            }
        }
    }
}
