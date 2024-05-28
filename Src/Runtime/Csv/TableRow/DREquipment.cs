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
public class DREquipment : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取dismantlingMats-int[][]。*/
    /// </summary>
    public int[][] DismantlingMats
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gearAttribute-int[][]。*/
    /// </summary>
    public int[][] GearAttribute
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gearAvatar-int。*/
    /// </summary>
    public int GearAvatar
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gearDurabilityMax-int。*/
    /// </summary>
    public int GearDurabilityMax
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gearQuality-int。*/
    /// </summary>
    public int GearQuality
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gearType-int。*/
    /// </summary>
    public int GearType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gender-string。*/
    /// </summary>
    public string Gender
    {
        get;
        private set;
    }

    /// <summary>
  /**获取givenSkillId-int[]。*/
    /// </summary>
    public int[] GivenSkillId
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
  /**获取weaponSubtype-int。*/
    /// </summary>
    public int WeaponSubtype
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        DismantlingMats = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        GearAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        GearAvatar = DataTableParseUtil.ParseInt(columnStrings[index++]);
        GearDurabilityMax = DataTableParseUtil.ParseInt(columnStrings[index++]);
        GearQuality = DataTableParseUtil.ParseInt(columnStrings[index++]);
        GearType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Gender = DataTableParseUtil.ParseString(columnStrings[index++]);
        GivenSkillId = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        ItemId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        WeaponSubtype = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                DismantlingMats = binaryReader.ReadArrayList<Int32>();
                GearAttribute = binaryReader.ReadArrayList<Int32>();
                GearAvatar = binaryReader.Read7BitEncodedInt32();
                GearDurabilityMax = binaryReader.Read7BitEncodedInt32();
                GearQuality = binaryReader.Read7BitEncodedInt32();
                GearType = binaryReader.Read7BitEncodedInt32();
                Gender = binaryReader.ReadString();
                GivenSkillId = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                ItemId = binaryReader.Read7BitEncodedInt32();
                WeaponSubtype = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

