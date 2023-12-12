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
public class DRNoviceGuide : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取customData-string。*/
    /// </summary>
    public string CustomData
    {
        get;
        private set;
    }

    /// <summary>
  /**获取event-string。*/
    /// </summary>
    public string Event
    {
        get;
        private set;
    }

    /// <summary>
  /**获取resList-string[][]。*/
    /// </summary>
    public string[][] ResList
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        CustomData = DataTableParseUtil.ParseString(columnStrings[index++]);
        Event = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        ResList = DataTableParseUtil.ParseArrayList<string>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                CustomData = binaryReader.ReadString();
                Event = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                ResList = binaryReader.ReadArrayList<String>();
            }
        }

        return true;
    }
}

