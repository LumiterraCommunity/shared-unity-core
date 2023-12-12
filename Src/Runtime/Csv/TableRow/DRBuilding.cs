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
public class DRBuilding : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取armatureRes-string。*/
    /// </summary>
    public string ArmatureRes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxCanCollect-int。*/
    /// </summary>
    public int MaxCanCollect
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxCanHarvest-int。*/
    /// </summary>
    public int MaxCanHarvest
    {
        get;
        private set;
    }

    /// <summary>
  /**获取powerCostPerHour-int。*/
    /// </summary>
    public int PowerCostPerHour
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rewardList-int[][]。*/
    /// </summary>
    public int[][] RewardList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取stolenpercentage-string。*/
    /// </summary>
    public string Stolenpercentage
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        ArmatureRes = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        MaxCanCollect = DataTableParseUtil.ParseInt(columnStrings[index++]);
        MaxCanHarvest = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PowerCostPerHour = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RewardList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Stolenpercentage = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ArmatureRes = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                MaxCanCollect = binaryReader.Read7BitEncodedInt32();
                MaxCanHarvest = binaryReader.Read7BitEncodedInt32();
                PowerCostPerHour = binaryReader.Read7BitEncodedInt32();
                RewardList = binaryReader.ReadArrayList<Int32>();
                Stolenpercentage = binaryReader.ReadString();
            }
        }

        return true;
    }
}

