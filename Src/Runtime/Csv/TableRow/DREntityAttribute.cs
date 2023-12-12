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
public class DREntityAttribute : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取defaultValue-int。*/
    /// </summary>
    public int DefaultValue
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
  /**获取isCore-boolean。*/
    /// </summary>
    public bool IsCore
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
  /**获取talentType-int。*/
    /// </summary>
    public int TalentType
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
  /**获取valueType-int。*/
    /// </summary>
    public int ValueType
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

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DefaultValue = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsCore = DataTableParseUtil.ParseBool(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        TalentType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ValueType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DefaultValue = binaryReader.Read7BitEncodedInt32();
                Desc = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                IsCore = binaryReader.ReadBoolean();
                Name = binaryReader.ReadString();
                TalentType = binaryReader.Read7BitEncodedInt32();
                Type = binaryReader.Read7BitEncodedInt32();
                ValueType = binaryReader.Read7BitEncodedInt32();
                Icon = binaryReader.ReadString();
            }
        }

        return true;
    }
}

