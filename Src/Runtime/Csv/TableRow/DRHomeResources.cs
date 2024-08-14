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
public class DRHomeResources : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取assetRes-string。*/
    /// </summary>
    public string AssetRes
    {
        get;
        private set;
    }

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
  /**获取homeAction-int[]。*/
    /// </summary>
    public int[] HomeAction
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
  /**获取initialAttribute-int[][]。*/
    /// </summary>
    public int[][] InitialAttribute
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
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取progressScore-int。*/
    /// </summary>
    public int ProgressScore
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
  /**获取tokenTaskLevel-int[]。*/
    /// </summary>
    public int[] TokenTaskLevel
    {
        get;
        private set;
    }

    /// <summary>
  /**获取totemScore-int。*/
    /// </summary>
    public int TotemScore
    {
        get;
        private set;
    }

    /// <summary>
  /**获取worldDropGroup-int。*/
    /// </summary>
    public int WorldDropGroup
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        AssetRes = DataTableParseUtil.ParseString(columnStrings[index++]);
        DropId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Exp = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HomeAction = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        InitialAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Lv = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        ProgressScore = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RankScore = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TokenTaskLevel = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        TotemScore = DataTableParseUtil.ParseInt(columnStrings[index++]);
        WorldDropGroup = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                AssetRes = binaryReader.ReadString();
                DropId = binaryReader.Read7BitEncodedInt32();
                Exp = binaryReader.Read7BitEncodedInt32();
                HomeAction = binaryReader.ReadArray<Int32>();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                InitialAttribute = binaryReader.ReadArrayList<Int32>();
                Lv = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                ProgressScore = binaryReader.Read7BitEncodedInt32();
                RankScore = binaryReader.Read7BitEncodedInt32();
                TokenTaskLevel = binaryReader.ReadArray<Int32>();
                TotemScore = binaryReader.Read7BitEncodedInt32();
                WorldDropGroup = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

