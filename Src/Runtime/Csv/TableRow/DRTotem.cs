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
public class DRTotem : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取defaultEnhanceFee-int[][]。*/
    /// </summary>
    public int[][] DefaultEnhanceFee
    {
        get;
        private set;
    }

    /// <summary>
  /**获取enhanceSucPro-int[]。*/
    /// </summary>
    public int[] EnhanceSucPro
    {
        get;
        private set;
    }

    /// <summary>
  /**获取quality-int。*/
    /// </summary>
    public int Quality
    {
        get;
        private set;
    }

    /// <summary>
  /**获取totemEntity-string。*/
    /// </summary>
    public string TotemEntity
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tpFee-int。*/
    /// </summary>
    public int TpFee
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DefaultEnhanceFee = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        EnhanceSucPro = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Quality = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TotemEntity = DataTableParseUtil.ParseString(columnStrings[index++]);
        TpFee = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DefaultEnhanceFee = binaryReader.ReadArrayList<Int32>();
                EnhanceSucPro = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                Quality = binaryReader.Read7BitEncodedInt32();
                TotemEntity = binaryReader.ReadString();
                TpFee = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

