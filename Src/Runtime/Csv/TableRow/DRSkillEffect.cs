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
public class DRSkillEffect : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取desc-string。*/
    /// </summary>
    public string Desc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取duration-int。*/
    /// </summary>
    public int Duration
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectType-int。*/
    /// </summary>
    public int EffectType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isRepeat-bool。*/
    /// </summary>
    public bool IsRepeat
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxLayer-int。*/
    /// </summary>
    public int MaxLayer
    {
        get;
        private set;
    }

    /// <summary>
  /**获取parameters-int[]。*/
    /// </summary>
    public int[] Parameters
    {
        get;
        private set;
    }

    /// <summary>
  /**获取showBuffIcon-bool。*/
    /// </summary>
    public bool ShowBuffIcon
    {
        get;
        private set;
    }

    /// <summary>
  /**获取parameters2-int[][]。*/
    /// </summary>
    public int[][] Parameters2
    {
        get;
        private set;
    }

    /// <summary>
  /**获取buffIcon-string。*/
    /// </summary>
    public string BuffIcon
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectInterval-int。*/
    /// </summary>
    public int EffectInterval
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectFlag-int[]。*/
    /// </summary>
    public int[] EffectFlag
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectImmuneFlag-int[]。*/
    /// </summary>
    public int[] EffectImmuneFlag
    {
        get;
        private set;
    }

    /// <summary>
  /**获取entityEffect-string。*/
    /// </summary>
    public string EntityEffect
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        Duration = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EffectType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsRepeat = DataTableParseUtil.ParseBool(columnStrings[index++]);
        MaxLayer = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Parameters = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ShowBuffIcon = DataTableParseUtil.ParseBool(columnStrings[index++]);
        Parameters2 = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        BuffIcon = DataTableParseUtil.ParseString(columnStrings[index++]);
        EffectInterval = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EffectFlag = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        EffectImmuneFlag = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        EntityEffect = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Desc = binaryReader.ReadString();
                Duration = binaryReader.Read7BitEncodedInt32();
                EffectType = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                IsRepeat = binaryReader.ReadBoolean();
                MaxLayer = binaryReader.Read7BitEncodedInt32();
                Parameters = binaryReader.ReadArray<Int32>();
                ShowBuffIcon = binaryReader.ReadBoolean();
                Parameters2 = binaryReader.ReadArrayList<Int32>();
                BuffIcon = binaryReader.ReadString();
                EffectInterval = binaryReader.Read7BitEncodedInt32();
                EffectFlag = binaryReader.ReadArray<Int32>();
                EffectImmuneFlag = binaryReader.ReadArray<Int32>();
                EntityEffect = binaryReader.ReadString();
            }
        }

        return true;
    }
}

