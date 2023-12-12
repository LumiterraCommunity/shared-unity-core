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
public class DRItemEatable : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取args-string[]。*/
    /// </summary>
    public string[] Args
    {
        get;
        private set;
    }

    /// <summary>
  /**获取callFunc-string[]。*/
    /// </summary>
    public string[] CallFunc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取cd-int。*/
    /// </summary>
    public int Cd
    {
        get;
        private set;
    }

    /// <summary>
  /**获取cdType-int。*/
    /// </summary>
    public int CdType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取interactType-string。*/
    /// </summary>
    public string InteractType
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Args = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        CallFunc = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        Cd = DataTableParseUtil.ParseInt(columnStrings[index++]);
        CdType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        InteractType = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Args = binaryReader.ReadArray<String>();
                CallFunc = binaryReader.ReadArray<String>();
                Cd = binaryReader.Read7BitEncodedInt32();
                CdType = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                InteractType = binaryReader.ReadString();
            }
        }

        return true;
    }
}

