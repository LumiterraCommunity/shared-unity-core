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
public class DRNpc : DataRowBase
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
  /**获取displayName-string。*/
    /// </summary>
    public string DisplayName
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
  /**获取moveSpeed-int。*/
    /// </summary>
    public int MoveSpeed
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
  /**获取roleAssetID-int。*/
    /// </summary>
    public int RoleAssetID
    {
        get;
        private set;
    }

    /// <summary>
  /**获取taskBegin-int[]。*/
    /// </summary>
    public int[] TaskBegin
    {
        get;
        private set;
    }

    /// <summary>
  /**获取taskEnd-int[]。*/
    /// </summary>
    public int[] TaskEnd
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        DisplayName = DataTableParseUtil.ParseString(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        MoveSpeed = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        RoleAssetID = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TaskBegin = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        TaskEnd = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Desc = binaryReader.ReadString();
                DisplayName = binaryReader.ReadString();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                MoveSpeed = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                RoleAssetID = binaryReader.Read7BitEncodedInt32();
                TaskBegin = binaryReader.ReadArray<Int32>();
                TaskEnd = binaryReader.ReadArray<Int32>();
            }
        }

        return true;
    }
}

