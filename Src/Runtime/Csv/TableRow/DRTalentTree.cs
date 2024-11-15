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
public class DRTalentTree : DataRowBase
{
  private int _id = 0;

  /// <summary>
  /// /**获取id-int。*/
  /// </summary>
  public override int Id => _id;

  /// <summary>
  /**获取col-int。*/
  /// </summary>
  public int Col
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
  /**获取gainsArgs-string。*/
  /// </summary>
  public string GainsArgs
  {
    get;
    private set;
  }

  /// <summary>
  /**获取gainsType-int[]。*/
  /// </summary>
  public int[] GainsType
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
  /**获取isTrunk-bool。*/
  /// </summary>
  public bool IsTrunk
  {
    get;
    private set;
  }

  /// <summary>
  /**获取layer-int。*/
  /// </summary>
  public int Layer
  {
    get;
    private set;
  }

  /// <summary>
  /**获取lvLimit-int。*/
  /// </summary>
  public int LvLimit
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
  /**获取preNode-int[]。*/
  /// </summary>
  public int[] PreNode
  {
    get;
    private set;
  }

  /// <summary>
  /**获取resetCost-int。*/
  /// </summary>
  public int ResetCost
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
  /**获取upgradeEXP-int[]。*/
  /// </summary>
  public int[] UpgradeEXP
  {
    get;
    private set;
  }

  /// <summary>
  /**获取upgradeRequireTreeLv-int。*/
  /// </summary>
  public int UpgradeRequireTreeLv
  {
    get;
    private set;
  }

  /// <summary>
  /**获取upgradeToken-int[]。*/
  /// </summary>
  public int[] UpgradeToken
  {
    get;
    private set;
  }

  public override bool ParseDataRow(string dataRowString, object userData)
  {
    string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

    int index = 0;
    Col = DataTableParseUtil.ParseInt(columnStrings[index++]);
    Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
    GainsArgs = DataTableParseUtil.ParseString(columnStrings[index++]);
    GainsType = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
    _id = int.Parse(columnStrings[index++]);
    IsTrunk = DataTableParseUtil.ParseBool(columnStrings[index++]);
    Layer = DataTableParseUtil.ParseInt(columnStrings[index++]);
    LvLimit = DataTableParseUtil.ParseInt(columnStrings[index++]);
    Name = DataTableParseUtil.ParseString(columnStrings[index++]);
    PreNode = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    ResetCost = DataTableParseUtil.ParseInt(columnStrings[index++]);
    Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
    UpgradeEXP = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    UpgradeRequireTreeLv = DataTableParseUtil.ParseInt(columnStrings[index++]);
    UpgradeToken = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);

    return true;
  }


  public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
  {
    using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
    {
      using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
      {
        Col = binaryReader.Read7BitEncodedInt32();
        Desc = binaryReader.ReadString();
        GainsArgs = binaryReader.ReadString();
        GainsType = binaryReader.ReadArray<Int32>();
        Icon = binaryReader.ReadString();
        _id = binaryReader.Read7BitEncodedInt32();
        IsTrunk = binaryReader.ReadBoolean();
        Layer = binaryReader.Read7BitEncodedInt32();
        LvLimit = binaryReader.Read7BitEncodedInt32();
        Name = binaryReader.ReadString();
        PreNode = binaryReader.ReadArray<Int32>();
        ResetCost = binaryReader.Read7BitEncodedInt32();
        Type = binaryReader.Read7BitEncodedInt32();
        UpgradeEXP = binaryReader.ReadArray<Int32>();
        UpgradeRequireTreeLv = binaryReader.Read7BitEncodedInt32();
        UpgradeToken = binaryReader.ReadArray<Int32>();
      }
    }

    return true;
  }

  public void SetPreNode(int[] preNode)
  {
    PreNode = preNode;
  }
}