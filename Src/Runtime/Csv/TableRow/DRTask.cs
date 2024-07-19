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
public class DRTask : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取designateOptions-string。*/
    /// </summary>
    public string DesignateOptions
    {
        get;
        private set;
    }

    /// <summary>
  /**获取details-string。*/
    /// </summary>
    public string Details
    {
        get;
        private set;
    }

    /// <summary>
  /**获取difficulty-int。*/
    /// </summary>
    public int Difficulty
    {
        get;
        private set;
    }

    /// <summary>
  /**获取expReward-int[][]。*/
    /// </summary>
    public int[][] ExpReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isSelfEnd-bool。*/
    /// </summary>
    public bool IsSelfEnd
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
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取reputation-int。*/
    /// </summary>
    public int Reputation
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
  /**获取level-int[][]。*/
    /// </summary>
    public int[][] Level
    {
        get;
        private set;
    }

    /// <summary>
  /**获取preTaskReq-int[]。*/
    /// </summary>
    public int[] PreTaskReq
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chanceOptions-string。*/
    /// </summary>
    public string ChanceOptions
    {
        get;
        private set;
    }

    /// <summary>
  /**获取unlockDungeon-int[]。*/
    /// </summary>
    public int[] UnlockDungeon
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DesignateOptions = DataTableParseUtil.ParseString(columnStrings[index++]);
        Details = DataTableParseUtil.ParseString(columnStrings[index++]);
        Difficulty = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ExpReward = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsSelfEnd = DataTableParseUtil.ParseBool(columnStrings[index++]);
        ItemReward = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        Reputation = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Level = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        PreTaskReq = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChanceOptions = DataTableParseUtil.ParseString(columnStrings[index++]);
        UnlockDungeon = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DesignateOptions = binaryReader.ReadString();
                Details = binaryReader.ReadString();
                Difficulty = binaryReader.Read7BitEncodedInt32();
                ExpReward = binaryReader.ReadArrayList<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                IsSelfEnd = binaryReader.ReadBoolean();
                ItemReward = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                Reputation = binaryReader.Read7BitEncodedInt32();
                Type = binaryReader.Read7BitEncodedInt32();
                Level = binaryReader.ReadArrayList<Int32>();
                PreTaskReq = binaryReader.ReadArray<Int32>();
                ChanceOptions = binaryReader.ReadString();
                UnlockDungeon = binaryReader.ReadArray<Int32>();
            }
        }

        return true;
    }
}

