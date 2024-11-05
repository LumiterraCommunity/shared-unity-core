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
public class DRMapping : DataRowBase
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
  /**获取itemId-int。*/
    /// </summary>
    public int ItemId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取petId-int。*/
    /// </summary>
    public int PetId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取text-string。*/
    /// </summary>
    public string Text
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        ItemId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PetId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Text = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Desc = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                ItemId = binaryReader.Read7BitEncodedInt32();
                PetId = binaryReader.Read7BitEncodedInt32();
                Text = binaryReader.ReadString();
            }
        }

        return true;
    }
}

