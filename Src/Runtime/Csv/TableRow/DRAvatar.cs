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
public class DRAvatar : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取resGirl-string。*/
    /// </summary>
    public string ResGirl
    {
        get;
        private set;
    }

    /// <summary>
  /**获取resIconGirl-string。*/
    /// </summary>
    public string ResIconGirl
    {
        get;
        private set;
    }

    /// <summary>
  /**获取avatarType-int。*/
    /// </summary>
    public int AvatarType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取resBoy-string。*/
    /// </summary>
    public string ResBoy
    {
        get;
        private set;
    }

    /// <summary>
  /**获取resIconBoy-string。*/
    /// </summary>
    public string ResIconBoy
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        _id = int.Parse(columnStrings[index++]);
        ResGirl = DataTableParseUtil.ParseString(columnStrings[index++]);
        ResIconGirl = DataTableParseUtil.ParseString(columnStrings[index++]);
        AvatarType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ResBoy = DataTableParseUtil.ParseString(columnStrings[index++]);
        ResIconBoy = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                _id = binaryReader.Read7BitEncodedInt32();
                ResGirl = binaryReader.ReadString();
                ResIconGirl = binaryReader.ReadString();
                AvatarType = binaryReader.Read7BitEncodedInt32();
                ResBoy = binaryReader.ReadString();
                ResIconBoy = binaryReader.ReadString();
            }
        }

        return true;
    }
}

