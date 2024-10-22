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
public class DRGeneralLotteryPool : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取chargeGap-int[]。*/
    /// </summary>
    public int[] ChargeGap
    {
        get;
        private set;
    }

    /// <summary>
  /**获取materials-int[][]。*/
    /// </summary>
    public int[][] Materials
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sceneAreaId-int。*/
    /// </summary>
    public int SceneAreaId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取token-int。*/
    /// </summary>
    public int Token
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        ChargeGap = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Materials = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        SceneAreaId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Token = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ChargeGap = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                Materials = binaryReader.ReadArrayList<Int32>();
                SceneAreaId = binaryReader.Read7BitEncodedInt32();
                Token = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

