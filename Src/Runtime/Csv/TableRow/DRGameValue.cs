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
public class DRGameValue : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取note-string。*/
    /// </summary>
    public string Note
    {
        get;
        private set;
    }

    /// <summary>
  /**获取value-int。*/
    /// </summary>
    public int Value
    {
        get;
        private set;
    }

    /// <summary>
  /**获取valueArray2-int[][]。*/
    /// </summary>
    public int[][] ValueArray2
    {
        get;
        private set;
    }

    /// <summary>
  /**获取strValue-string。*/
    /// </summary>
    public string StrValue
    {
        get;
        private set;
    }

    /// <summary>
  /**获取valueArray-int[]。*/
    /// </summary>
    public int[] ValueArray
    {
        get;
        private set;
    }

    /// <summary>
  /**获取strValueArray-string[]。*/
    /// </summary>
    public string[] StrValueArray
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        _id = int.Parse(columnStrings[index++]);
        Note = DataTableParseUtil.ParseString(columnStrings[index++]);
        Value = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ValueArray2 = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        StrValue = DataTableParseUtil.ParseString(columnStrings[index++]);
        ValueArray = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        StrValueArray = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                _id = binaryReader.Read7BitEncodedInt32();
                Note = binaryReader.ReadString();
                Value = binaryReader.Read7BitEncodedInt32();
                ValueArray2 = binaryReader.ReadArrayList<Int32>();
                StrValue = binaryReader.ReadString();
                ValueArray = binaryReader.ReadArray<Int32>();
                StrValueArray = binaryReader.ReadArray<String>();
            }
        }

        return true;
    }
}

