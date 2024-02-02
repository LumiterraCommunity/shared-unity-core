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
public class DRBattleArea : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取deathDrop-int[]。*/
    /// </summary>
    public int[] DeathDrop
    {
        get;
        private set;
    }

    /// <summary>
  /**获取deathEXPDrop-int。*/
    /// </summary>
    public int DeathEXPDrop
    {
        get;
        private set;
    }

    /// <summary>
  /**获取deathTaxation-int[]。*/
    /// </summary>
    public int[] DeathTaxation
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
        DeathDrop = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        DeathEXPDrop = DataTableParseUtil.ParseInt(columnStrings[index++]);
        DeathTaxation = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
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
                DeathDrop = binaryReader.ReadArray<Int32>();
                DeathEXPDrop = binaryReader.Read7BitEncodedInt32();
                DeathTaxation = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                Type = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

