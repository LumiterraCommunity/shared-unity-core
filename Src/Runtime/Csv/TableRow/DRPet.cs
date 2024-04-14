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
public class DRPet : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取ability-int[][]。*/
    /// </summary>
    public int[][] Ability
    {
        get;
        private set;
    }

    /// <summary>
  /**获取autoHarvest-bool。*/
    /// </summary>
    public bool AutoHarvest
    {
        get;
        private set;
    }

    /// <summary>
  /**获取breedingDifficulty-int。*/
    /// </summary>
    public int BreedingDifficulty
    {
        get;
        private set;
    }

    /// <summary>
  /**获取eggItemId-int。*/
    /// </summary>
    public int EggItemId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取harvestAction-int。*/
    /// </summary>
    public int HarvestAction
    {
        get;
        private set;
    }

    /// <summary>
  /**获取harvestDropId-int。*/
    /// </summary>
    public int HarvestDropId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取harvestTime-int。*/
    /// </summary>
    public int HarvestTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取hungerSpeed-int。*/
    /// </summary>
    public int HungerSpeed
    {
        get;
        private set;
    }

    /// <summary>
  /**获取incubationDuration-int。*/
    /// </summary>
    public int IncubationDuration
    {
        get;
        private set;
    }

    /// <summary>
  /**获取incubationMat-int[][]。*/
    /// </summary>
    public int[][] IncubationMat
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
  /**获取maxHunger-int。*/
    /// </summary>
    public int MaxHunger
    {
        get;
        private set;
    }

    /// <summary>
  /**获取potWashingItem-int[]。*/
    /// </summary>
    public int[] PotWashingItem
    {
        get;
        private set;
    }

    /// <summary>
  /**获取requiredHappiness-int。*/
    /// </summary>
    public int RequiredHappiness
    {
        get;
        private set;
    }

    /// <summary>
  /**获取requiredProficiency-int。*/
    /// </summary>
    public int RequiredProficiency
    {
        get;
        private set;
    }

    /// <summary>
  /**获取toEggProbability-int。*/
    /// </summary>
    public int ToEggProbability
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
  /**获取extendSkill-int[]。*/
    /// </summary>
    public int[] ExtendSkill
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        Ability = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        AutoHarvest = DataTableParseUtil.ParseBool(columnStrings[index++]);
        BreedingDifficulty = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EggItemId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HarvestAction = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HarvestDropId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HarvestTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        HungerSpeed = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IncubationDuration = DataTableParseUtil.ParseInt(columnStrings[index++]);
        IncubationMat = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        InitialAttribute = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        MaxHunger = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PotWashingItem = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        RequiredHappiness = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RequiredProficiency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ToEggProbability = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UpgradeEXP = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ExtendSkill = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                Ability = binaryReader.ReadArrayList<Int32>();
                AutoHarvest = binaryReader.ReadBoolean();
                BreedingDifficulty = binaryReader.Read7BitEncodedInt32();
                EggItemId = binaryReader.Read7BitEncodedInt32();
                HarvestAction = binaryReader.Read7BitEncodedInt32();
                HarvestDropId = binaryReader.Read7BitEncodedInt32();
                HarvestTime = binaryReader.Read7BitEncodedInt32();
                HungerSpeed = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                IncubationDuration = binaryReader.Read7BitEncodedInt32();
                IncubationMat = binaryReader.ReadArrayList<Int32>();
                InitialAttribute = binaryReader.ReadArrayList<Int32>();
                MaxHunger = binaryReader.Read7BitEncodedInt32();
                PotWashingItem = binaryReader.ReadArray<Int32>();
                RequiredHappiness = binaryReader.Read7BitEncodedInt32();
                RequiredProficiency = binaryReader.Read7BitEncodedInt32();
                ToEggProbability = binaryReader.Read7BitEncodedInt32();
                UpgradeEXP = binaryReader.ReadArray<Int32>();
                ExtendSkill = binaryReader.ReadArray<Int32>();
            }
        }

        return true;
    }
}

