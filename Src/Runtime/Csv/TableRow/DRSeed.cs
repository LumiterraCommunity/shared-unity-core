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
public class DRSeed : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取dropId-int。*/
    /// </summary>
    public int DropId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取exp-int。*/
    /// </summary>
    public int Exp
    {
        get;
        private set;
    }

    /// <summary>
  /**获取functionType-int。*/
    /// </summary>
    public int FunctionType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取growRes-string[]。*/
    /// </summary>
    public string[] GrowRes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取growTotalTime-int。*/
    /// </summary>
    public int GrowTotalTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取harvestRes-string。*/
    /// </summary>
    public string HarvestRes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取lv-int。*/
    /// </summary>
    public int Lv
    {
        get;
        private set;
    }

    /// <summary>
  /**获取needWaterValue-int。*/
    /// </summary>
    public int NeedWaterValue
    {
        get;
        private set;
    }

    /// <summary>
  /**获取plantingDifficulty-int。*/
    /// </summary>
    public int PlantingDifficulty
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rankScore-int。*/
    /// </summary>
    public int RankScore
    {
        get;
        private set;
    }

    /// <summary>
  /**获取requiredFertilizer-int。*/
    /// </summary>
    public int RequiredFertilizer
    {
        get;
        private set;
    }

    /// <summary>
  /**获取requirementProficiency-int。*/
    /// </summary>
    public int RequirementProficiency
    {
        get;
        private set;
    }

    /// <summary>
  /**获取witherTime-int。*/
    /// </summary>
    public int WitherTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取functionAvatar-string。*/
    /// </summary>
    public string FunctionAvatar
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DropId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Exp = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FunctionType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        GrowRes = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        GrowTotalTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HarvestRes = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Lv = DataTableParseUtil.ParseInt(columnStrings[index++]);
        NeedWaterValue = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PlantingDifficulty = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RankScore = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RequiredFertilizer = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RequirementProficiency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        WitherTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FunctionAvatar = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DropId = binaryReader.Read7BitEncodedInt32();
                Exp = binaryReader.Read7BitEncodedInt32();
                FunctionType = binaryReader.Read7BitEncodedInt32();
                GrowRes = binaryReader.ReadArray<String>();
                GrowTotalTime = binaryReader.Read7BitEncodedInt32();
                HarvestRes = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Lv = binaryReader.Read7BitEncodedInt32();
                NeedWaterValue = binaryReader.Read7BitEncodedInt32();
                PlantingDifficulty = binaryReader.Read7BitEncodedInt32();
                RankScore = binaryReader.Read7BitEncodedInt32();
                RequiredFertilizer = binaryReader.Read7BitEncodedInt32();
                RequirementProficiency = binaryReader.Read7BitEncodedInt32();
                WitherTime = binaryReader.Read7BitEncodedInt32();
                FunctionAvatar = binaryReader.ReadString();
            }
        }

        return true;
    }
}

