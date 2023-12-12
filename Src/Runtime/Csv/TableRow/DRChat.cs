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
public class DRChat : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取channelRGB-string。*/
    /// </summary>
    public string ChannelRGB
    {
        get;
        private set;
    }

    /// <summary>
  /**获取ifBubble-bool。*/
    /// </summary>
    public bool IfBubble
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxTips-int。*/
    /// </summary>
    public int MaxTips
    {
        get;
        private set;
    }

    /// <summary>
  /**获取talkCD-int。*/
    /// </summary>
    public int TalkCD
    {
        get;
        private set;
    }

    /// <summary>
  /**获取talkNeedGold-int。*/
    /// </summary>
    public int TalkNeedGold
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        ChannelRGB = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IfBubble = DataTableParseUtil.ParseBool(columnStrings[index++]);
        MaxTips = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TalkCD = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TalkNeedGold = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ChannelRGB = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                IfBubble = binaryReader.ReadBoolean();
                MaxTips = binaryReader.Read7BitEncodedInt32();
                TalkCD = binaryReader.Read7BitEncodedInt32();
                TalkNeedGold = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

