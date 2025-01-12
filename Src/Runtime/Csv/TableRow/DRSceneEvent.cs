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
public class DRSceneEvent : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取actions-int[]。*/
    /// </summary>
    public int[] Actions
    {
        get;
        private set;
    }

    /// <summary>
  /**获取conditionType-int。*/
    /// </summary>
    public int ConditionType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取conditions-int[]。*/
    /// </summary>
    public int[] Conditions
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
  /**获取name-string。*/
    /// </summary>
    public string Name
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
        Actions = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ConditionType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Conditions = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Actions = binaryReader.ReadArray<Int32>();
                ConditionType = binaryReader.Read7BitEncodedInt32();
                Conditions = binaryReader.ReadArray<Int32>();
                Desc = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                Type = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

