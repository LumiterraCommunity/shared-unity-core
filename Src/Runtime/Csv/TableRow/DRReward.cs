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
public class DRReward : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取rewardList-int[][]。*/
    /// </summary>
    public int[][] RewardList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rewardTimes-int[][]。*/
    /// </summary>
    public int[][] RewardTimes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rewardType-int。*/
    /// </summary>
    public int RewardType
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        _id = int.Parse(columnStrings[index++]);
        RewardList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        RewardTimes = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        RewardType = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                _id = binaryReader.Read7BitEncodedInt32();
                RewardList = binaryReader.ReadArrayList<Int32>();
                RewardTimes = binaryReader.ReadArrayList<Int32>();
                RewardType = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

