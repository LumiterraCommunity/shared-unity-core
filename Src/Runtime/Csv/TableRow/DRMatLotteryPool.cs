﻿//------------------------------------------------------------
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
public class DRMatLotteryPool : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取dailyItemList-int[][]。*/
    /// </summary>
    public int[][] DailyItemList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取desc-string。*/
    /// </summary>
    public string Desc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取distributionWeight-int[]。*/
    /// </summary>
    public int[] DistributionWeight
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isActive-bool。*/
    /// </summary>
    public bool IsActive
    {
        get;
        private set;
    }

    /// <summary>
  /**获取levelRangeIdx-int。*/
    /// </summary>
    public int LevelRangeIdx
    {
        get;
        private set;
    }

    /// <summary>
  /**获取lotteryItem-int。*/
    /// </summary>
    public int LotteryItem
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
  /**获取talentType-int。*/
    /// </summary>
    public int TalentType
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DailyItemList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        DistributionWeight = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsActive = DataTableParseUtil.ParseBool(columnStrings[index++]);
        LevelRangeIdx = DataTableParseUtil.ParseInt(columnStrings[index++]);
        LotteryItem = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        TalentType = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DailyItemList = binaryReader.ReadArrayList<Int32>();
                Desc = binaryReader.ReadString();
                DistributionWeight = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                IsActive = binaryReader.ReadBoolean();
                LevelRangeIdx = binaryReader.Read7BitEncodedInt32();
                LotteryItem = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                TalentType = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

