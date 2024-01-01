//------------------------------------------------------------
// 此文件由工具自动生成
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;


/// <summary>
/** __DATA_TABLE_COMMENT__*/
/// </summary>
public class DRLanguage : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取describe-string。*/
    /// </summary>
    public string Describe
    {
        get;
        private set;
    }

    /// <summary>
  /**获取module-string。*/
    /// </summary>
    public string Module
    {
        get;
        private set;
    }

    /// <summary>
  /**获取value-string。*/
    /// </summary>
    public string Value
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Describe = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Module = DataTableParseUtil.ParseString(columnStrings[index++]);
        Value = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Describe = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Module = binaryReader.ReadString();
                Value = binaryReader.ReadString();
            }
        }

        return true;
    }
}

