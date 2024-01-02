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
public class DRTaskList : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取expReward-int[][]。*/
    /// </summary>
    public int[][] ExpReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取itemReward-int。*/
    /// </summary>
    public int ItemReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取level-int。*/
    /// </summary>
    public int Level
    {
        get;
        private set;
    }

    /// <summary>
  /**获取progressReset-int。*/
    /// </summary>
    public int ProgressReset
    {
        get;
        private set;
    }

    /// <summary>
  /**获取system-int。*/
    /// </summary>
    public int System
    {
        get;
        private set;
    }

    /// <summary>
  /**获取taskPool-int[][]。*/
    /// </summary>
    public int[][] TaskPool
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
        ExpReward = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        ItemReward = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Level = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ProgressReset = DataTableParseUtil.ParseInt(columnStrings[index++]);
        System = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TaskPool = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ExpReward = binaryReader.ReadArrayList<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                ItemReward = binaryReader.Read7BitEncodedInt32();
                Level = binaryReader.Read7BitEncodedInt32();
                ProgressReset = binaryReader.Read7BitEncodedInt32();
                System = binaryReader.Read7BitEncodedInt32();
                TaskPool = binaryReader.ReadArrayList<Int32>();
                Type = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

