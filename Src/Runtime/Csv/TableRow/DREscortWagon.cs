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
public class DREscortWagon : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

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
  /**获取lvLimit-int。*/
    /// </summary>
    public int LvLimit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxAccept-int。*/
    /// </summary>
    public int MaxAccept
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxExistenceTime-int。*/
    /// </summary>
    public int MaxExistenceTime
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
  /**获取rewardId-int。*/
    /// </summary>
    public int RewardId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取roleAssetId-int。*/
    /// </summary>
    public int RoleAssetId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tickets-int[][]。*/
    /// </summary>
    public int[][] Tickets
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        _id = int.Parse(columnStrings[index++]);
        InitialAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Lv = DataTableParseUtil.ParseInt(columnStrings[index++]);
        LvLimit = DataTableParseUtil.ParseInt(columnStrings[index++]);
        MaxAccept = DataTableParseUtil.ParseInt(columnStrings[index++]);
        MaxExistenceTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        RewardId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RoleAssetId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Tickets = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                _id = binaryReader.Read7BitEncodedInt32();
                InitialAttribute = binaryReader.ReadArrayList<Int32>();
                Lv = binaryReader.Read7BitEncodedInt32();
                LvLimit = binaryReader.Read7BitEncodedInt32();
                MaxAccept = binaryReader.Read7BitEncodedInt32();
                MaxExistenceTime = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                RewardId = binaryReader.Read7BitEncodedInt32();
                RoleAssetId = binaryReader.Read7BitEncodedInt32();
                Tickets = binaryReader.ReadArrayList<Int32>();
            }
        }

        return true;
    }
}

