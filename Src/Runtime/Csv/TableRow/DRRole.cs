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
public class DRRole : DataRowBase
{
  private int _id = 0;

  /// <summary>
  /// /**获取id-int。*/
  /// </summary>
  public override int Id => _id;

  /// <summary>
  /**获取RoleActionSkill-int[]。*/
  /// </summary>
  public int[] RoleActionSkill
  {
    get;
    private set;
  }

  /// <summary>
  /**获取bodyCapacity-int。*/
  /// </summary>
  public int BodyCapacity
  {
    get;
    private set;
  }

  /// <summary>
  /**获取grasslandRunSound-string。*/
  /// </summary>
  public string GrasslandRunSound
  {
    get;
    private set;
  }

  /// <summary>
  /**获取harvestSkill-int。*/
  /// </summary>
  public int HarvestSkill
  {
    get;
    private set;
  }

  /// <summary>
  /**获取initialAttribute-int[][]。*/
  /// </summary>
  public int[][] InitialAttribute
  {
    get;
    private set;
  }

  /// <summary>
  /**获取jumpRollSkill-int。*/
  /// </summary>
  public int JumpRollSkill
  {
    get;
    private set;
  }

  /// <summary>
  /**获取pickUpSound-string。*/
  /// </summary>
  public string PickUpSound
  {
    get;
    private set;
  }

  /// <summary>
  /**获取randomAnim-string[]。*/
  /// </summary>
  public string[] RandomAnim
  {
    get;
    private set;
  }

  /// <summary>
  /**获取rescueSkill-int。*/
  /// </summary>
  public int RescueSkill
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
  /**获取roleDefaultAvatar-int[]。*/
  /// </summary>
  public int[] RoleDefaultAvatar
  {
    get;
    private set;
  }

  /// <summary>
  /**获取roleDefaultSkill-int[]。*/
  /// </summary>
  public int[] RoleDefaultSkill
  {
    get;
    private set;
  }

  /// <summary>
  /**获取roleIcon-string[]。*/
  /// </summary>
  public string[] RoleIcon
  {
    get;
    private set;
  }

  /// <summary>
  /**获取roleName-string。*/
  /// </summary>
  public string RoleName
  {
    get;
    private set;
  }

  /// <summary>
  /**获取roleSex-int。*/
  /// </summary>
  public int RoleSex
  {
    get;
    private set;
  }

  public override bool ParseDataRow(string dataRowString, object userData)
  {
    string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

    int index = 0;
    RoleActionSkill = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    BodyCapacity = DataTableParseUtil.ParseInt(columnStrings[index++]);
    GrasslandRunSound = DataTableParseUtil.ParseString(columnStrings[index++]);
    HarvestSkill = DataTableParseUtil.ParseInt(columnStrings[index++]);
    _id = int.Parse(columnStrings[index++]);
    InitialAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
    JumpRollSkill = DataTableParseUtil.ParseInt(columnStrings[index++]);
    PickUpSound = DataTableParseUtil.ParseString(columnStrings[index++]);
    RandomAnim = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
    RescueSkill = DataTableParseUtil.ParseInt(columnStrings[index++]);
    RoleAssetID = DataTableParseUtil.ParseInt(columnStrings[index++]);
    RoleDefaultAvatar = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    RoleDefaultSkill = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
    RoleIcon = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
    RoleName = DataTableParseUtil.ParseString(columnStrings[index++]);
    RoleSex = DataTableParseUtil.ParseInt(columnStrings[index++]);

    return true;
  }


  public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
  {
    using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
    {
      using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
      {
        RoleActionSkill = binaryReader.ReadArray<Int32>();
        BodyCapacity = binaryReader.Read7BitEncodedInt32();
        GrasslandRunSound = binaryReader.ReadString();
        HarvestSkill = binaryReader.Read7BitEncodedInt32();
        _id = binaryReader.Read7BitEncodedInt32();
        InitialAttribute = binaryReader.ReadArrayList<Int32>();
        JumpRollSkill = binaryReader.Read7BitEncodedInt32();
        PickUpSound = binaryReader.ReadString();
        RandomAnim = binaryReader.ReadArray<String>();
        RescueSkill = binaryReader.Read7BitEncodedInt32();
        RoleAssetID = binaryReader.Read7BitEncodedInt32();
        RoleDefaultAvatar = binaryReader.ReadArray<Int32>();
        RoleDefaultSkill = binaryReader.ReadArray<Int32>();
        RoleIcon = binaryReader.ReadArray<String>();
        RoleName = binaryReader.ReadString();
        RoleSex = binaryReader.Read7BitEncodedInt32();
      }
    }

    return true;
  }
}

