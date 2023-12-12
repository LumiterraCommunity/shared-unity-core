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
public class DRBuildingBatteryReCharge : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取battery-int。*/
    /// </summary>
    public int Battery
    {
        get;
        private set;
    }

    /// <summary>
  /**获取presentBattery-int。*/
    /// </summary>
    public int PresentBattery
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tokenCost-int。*/
    /// </summary>
    public int TokenCost
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Battery = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        PresentBattery = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TokenCost = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Battery = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                PresentBattery = binaryReader.Read7BitEncodedInt32();
                TokenCost = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

