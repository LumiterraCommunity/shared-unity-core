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
public class DRHomeResourceArea : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取isUpdateNow-bool。*/
    /// </summary>
    public bool IsUpdateNow
    {
        get;
        private set;
    }

    /// <summary>
  /**获取pointList-int[][]。*/
    /// </summary>
    public int[][] PointList
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

    /// <summary>
  /**获取updateInterval-int。*/
    /// </summary>
    public int UpdateInterval
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        _id = int.Parse(columnStrings[index++]);
        IsUpdateNow = DataTableParseUtil.ParseBool(columnStrings[index++]);
        PointList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UpdateInterval = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                _id = binaryReader.Read7BitEncodedInt32();
                IsUpdateNow = binaryReader.ReadBoolean();
                PointList = binaryReader.ReadArrayList<Int32>();
                Type = binaryReader.Read7BitEncodedInt32();
                UpdateInterval = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

