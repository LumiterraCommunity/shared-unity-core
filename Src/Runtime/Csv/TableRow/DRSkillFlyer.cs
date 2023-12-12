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
public class DRSkillFlyer : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取flyAvatar-string。*/
    /// </summary>
    public string FlyAvatar
    {
        get;
        private set;
    }

    /// <summary>
  /**获取flyDistance-int。*/
    /// </summary>
    public int FlyDistance
    {
        get;
        private set;
    }

    /// <summary>
  /**获取flySpeed-int。*/
    /// </summary>
    public int FlySpeed
    {
        get;
        private set;
    }

    /// <summary>
  /**获取flyTime-int。*/
    /// </summary>
    public int FlyTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取flyerType-int。*/
    /// </summary>
    public int FlyerType
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        FlyAvatar = DataTableParseUtil.ParseString(columnStrings[index++]);
        FlyDistance = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FlySpeed = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FlyTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FlyerType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                FlyAvatar = binaryReader.ReadString();
                FlyDistance = binaryReader.Read7BitEncodedInt32();
                FlySpeed = binaryReader.Read7BitEncodedInt32();
                FlyTime = binaryReader.Read7BitEncodedInt32();
                FlyerType = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

