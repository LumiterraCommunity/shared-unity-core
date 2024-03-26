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
public class DRMonster : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取bodyCapacity-int。*/
    /// </summary>
    public int BodyCapacity
    {
        get;
        private set;
    }

    /// <summary>
  /**获取captureHp-int。*/
    /// </summary>
    public int CaptureHp
    {
        get;
        private set;
    }

    /// <summary>
  /**获取captureSkillCastPool-int[][]。*/
    /// </summary>
    public int[][] CaptureSkillCastPool
    {
        get;
        private set;
    }

    /// <summary>
  /**获取combatDist-int。*/
    /// </summary>
    public int CombatDist
    {
        get;
        private set;
    }

    /// <summary>
  /**获取combatPotentiality-int[]。*/
    /// </summary>
    public int[] CombatPotentiality
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
  /**获取exp-int。*/
    /// </summary>
    public int Exp
    {
        get;
        private set;
    }

    /// <summary>
  /**获取farmPotentiality-int[]。*/
    /// </summary>
    public int[] FarmPotentiality
    {
        get;
        private set;
    }

    /// <summary>
  /**获取favoriteItem-int[]。*/
    /// </summary>
    public int[] FavoriteItem
    {
        get;
        private set;
    }

    /// <summary>
  /**获取gatherPotentiality-int[]。*/
    /// </summary>
    public int[] GatherPotentiality
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
  /**获取initialAttribute-int[][]。*/
    /// </summary>
    public int[][] InitialAttribute
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isTotemReward-bool。*/
    /// </summary>
    public bool IsTotemReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取lockEnemyRange-int。*/
    /// </summary>
    public int LockEnemyRange
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
  /**获取petId-int。*/
    /// </summary>
    public int PetId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取pushDist-int。*/
    /// </summary>
    public int PushDist
    {
        get;
        private set;
    }

    /// <summary>
  /**获取pushDmg-int。*/
    /// </summary>
    public int PushDmg
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
  /**获取skillCastPool-int[][]。*/
    /// </summary>
    public int[][] SkillCastPool
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

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        BodyCapacity = DataTableParseUtil.ParseInt(columnStrings[index++]);
        CaptureHp = DataTableParseUtil.ParseInt(columnStrings[index++]);
        CaptureSkillCastPool = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        CombatDist = DataTableParseUtil.ParseInt(columnStrings[index++]);
        CombatPotentiality = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        Exp = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FarmPotentiality = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        FavoriteItem = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        GatherPotentiality = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        InitialAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        IsTotemReward = DataTableParseUtil.ParseBool(columnStrings[index++]);
        LockEnemyRange = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        PetId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PushDist = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PushDmg = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RoleAssetID = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SkillCastPool = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        DropId = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                BodyCapacity = binaryReader.Read7BitEncodedInt32();
                CaptureHp = binaryReader.Read7BitEncodedInt32();
                CaptureSkillCastPool = binaryReader.ReadArrayList<Int32>();
                CombatDist = binaryReader.Read7BitEncodedInt32();
                CombatPotentiality = binaryReader.ReadArray<Int32>();
                Desc = binaryReader.ReadString();
                Exp = binaryReader.Read7BitEncodedInt32();
                FarmPotentiality = binaryReader.ReadArray<Int32>();
                FavoriteItem = binaryReader.ReadArray<Int32>();
                GatherPotentiality = binaryReader.ReadArray<Int32>();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                InitialAttribute = binaryReader.ReadArrayList<Int32>();
                IsTotemReward = binaryReader.ReadBoolean();
                LockEnemyRange = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                PetId = binaryReader.Read7BitEncodedInt32();
                PushDist = binaryReader.Read7BitEncodedInt32();
                PushDmg = binaryReader.Read7BitEncodedInt32();
                RoleAssetID = binaryReader.Read7BitEncodedInt32();
                SkillCastPool = binaryReader.ReadArrayList<Int32>();
                DropId = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

