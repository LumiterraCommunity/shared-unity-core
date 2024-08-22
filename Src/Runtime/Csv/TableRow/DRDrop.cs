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
public class DRDrop : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取dropList-int[][]。*/
    /// </summary>
    public int[][] DropList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxSharer-int。*/
    /// </summary>
    public int MaxSharer
    {
        get;
        private set;
    }

    /// <summary>
  /**获取extraDropList-int[][]。*/
    /// </summary>
    public int[][] ExtraDropList
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DropList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        MaxSharer = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ExtraDropList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DropList = binaryReader.ReadArrayList<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                MaxSharer = binaryReader.Read7BitEncodedInt32();
                ExtraDropList = binaryReader.ReadArrayList<Int32>();
            }
        }

        return true;
    }
}

