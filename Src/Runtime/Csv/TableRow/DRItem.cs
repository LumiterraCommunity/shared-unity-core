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
public class DRItem : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取boothFees-int[][]。*/
    /// </summary>
    public int[][] BoothFees
    {
        get;
        private set;
    }

    /// <summary>
  /**获取canDropScene-int[]。*/
    /// </summary>
    public int[] CanDropScene
    {
        get;
        private set;
    }

    /// <summary>
  /**获取canMint-int。*/
    /// </summary>
    public int CanMint
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
  /**获取givenSkillId-int。*/
    /// </summary>
    public int GivenSkillId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取icon-string。*/
    /// </summary>
    public string Icon
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
  /**获取quality-int[]。*/
    /// </summary>
    public int[] Quality
    {
        get;
        private set;
    }

    /// <summary>
  /**获取resourceInstructions-int[]。*/
    /// </summary>
    public int[] ResourceInstructions
    {
        get;
        private set;
    }

    /// <summary>
  /**获取subType-int。*/
    /// </summary>
    public int SubType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取talentId-int[]。*/
    /// </summary>
    public int[] TalentId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tokenTaskLevel-int[]。*/
    /// </summary>
    public int[] TokenTaskLevel
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tokenTaskRefreshType-int。*/
    /// </summary>
    public int TokenTaskRefreshType
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
  /**获取useLv-int。*/
    /// </summary>
    public int UseLv
    {
        get;
        private set;
    }

    /// <summary>
  /**获取userType-int。*/
    /// </summary>
    public int UserType
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        BoothFees = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        CanDropScene = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        CanMint = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        GivenSkillId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        Quality = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ResourceInstructions = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        SubType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TalentId = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        TokenTaskLevel = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        TokenTaskRefreshType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UseLv = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UserType = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                BoothFees = binaryReader.ReadArrayList<Int32>();
                CanDropScene = binaryReader.ReadArray<Int32>();
                CanMint = binaryReader.Read7BitEncodedInt32();
                Desc = binaryReader.ReadString();
                GivenSkillId = binaryReader.Read7BitEncodedInt32();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                Quality = binaryReader.ReadArray<Int32>();
                ResourceInstructions = binaryReader.ReadArray<Int32>();
                SubType = binaryReader.Read7BitEncodedInt32();
                TalentId = binaryReader.ReadArray<Int32>();
                TokenTaskLevel = binaryReader.ReadArray<Int32>();
                TokenTaskRefreshType = binaryReader.Read7BitEncodedInt32();
                Type = binaryReader.Read7BitEncodedInt32();
                UseLv = binaryReader.Read7BitEncodedInt32();
                UserType = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

