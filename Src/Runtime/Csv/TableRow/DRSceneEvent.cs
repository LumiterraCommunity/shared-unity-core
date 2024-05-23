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
public class DRSceneEvent : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取desc-string。*/
    /// </summary>
    public string Desc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取parameters_1-int[]。*/
    /// </summary>
    public int[] Parameters_1
    {
        get;
        private set;
    }

    /// <summary>
  /**获取parameters_2-int[][]。*/
    /// </summary>
    public int[][] Parameters_2
    {
        get;
        private set;
    }

    /// <summary>
  /**获取type-int。*/
    /// </summary>
    public int Type
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        Parameters_1 = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Parameters_2 = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Desc = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                Parameters_1 = binaryReader.ReadArray<Int32>();
                Parameters_2 = binaryReader.ReadArrayList<Int32>();
                Type = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

