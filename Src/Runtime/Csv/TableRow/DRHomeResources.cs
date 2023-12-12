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
  /**获取grassDef-int。*/
    /// </summary>
    public int GrassDef
    {
        get;
        private set;
    }

    /// <summary>
  /**获取homeAction-int。*/
    /// </summary>
    public int HomeAction
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
  /**获取lv-int。*/
    /// </summary>
    public int Lv
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxActionValue-int。*/
    /// </summary>
    public int MaxActionValue
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
  /**获取oreDef-int。*/
    /// </summary>
    public int OreDef
    {
        get;
        private set;
    }

    /// <summary>
  /**获取requiredProficiency-int。*/
    /// </summary>
    public int RequiredProficiency
    {
        get;
        private set;
    }

    /// <summary>
  /**获取treeDef-int。*/
    /// </summary>
    public int TreeDef
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
        GrassDef = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HomeAction = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Lv = DataTableParseUtil.ParseInt(columnStrings[index++]);
        MaxActionValue = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        OreDef = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RequiredProficiency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TreeDef = DataTableParseUtil.ParseInt(columnStrings[index++]);

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
                GrassDef = binaryReader.Read7BitEncodedInt32();
                HomeAction = binaryReader.Read7BitEncodedInt32();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Lv = binaryReader.Read7BitEncodedInt32();
                MaxActionValue = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                OreDef = binaryReader.Read7BitEncodedInt32();
                RequiredProficiency = binaryReader.Read7BitEncodedInt32();
                TreeDef = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

