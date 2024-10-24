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
public class DRRoleAsset : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取armatureRes-string。*/
    /// </summary>
    public string ArmatureRes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取attackSound-string。*/
    /// </summary>
    public string AttackSound
    {
        get;
        private set;
    }

    /// <summary>
  /**获取deathSound-string。*/
    /// </summary>
    public string DeathSound
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
  /**获取hurtedCritSound-string。*/
    /// </summary>
    public string HurtedCritSound
    {
        get;
        private set;
    }

    /// <summary>
  /**获取hurtedSound-string。*/
    /// </summary>
    public string HurtedSound
    {
        get;
        private set;
    }

    /// <summary>
  /**获取patrolRadius-int。*/
    /// </summary>
    public int PatrolRadius
    {
        get;
        private set;
    }

    /// <summary>
  /**获取patrolSpd-int。*/
    /// </summary>
    public int PatrolSpd
    {
        get;
        private set;
    }

    /// <summary>
  /**获取idleSound-string。*/
    /// </summary>
    public string IdleSound
    {
        get;
        private set;
    }

    /// <summary>
  /**获取aiName-string。*/
    /// </summary>
    public string AiName
    {
        get;
        private set;
    }

    /// <summary>
  /**获取patrolPath-string[]。*/
    /// </summary>
    public string PatrolPath
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        ArmatureRes = DataTableParseUtil.ParseString(columnStrings[index++]);
        AttackSound = DataTableParseUtil.ParseString(columnStrings[index++]);
        DeathSound = DataTableParseUtil.ParseString(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        HurtedCritSound = DataTableParseUtil.ParseString(columnStrings[index++]);
        HurtedSound = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        PatrolRadius = DataTableParseUtil.ParseInt(columnStrings[index++]);
        PatrolSpd = DataTableParseUtil.ParseInt(columnStrings[index++]);
        IdleSound = DataTableParseUtil.ParseString(columnStrings[index++]);
        AiName = DataTableParseUtil.ParseString(columnStrings[index++]);
        PatrolPath = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ArmatureRes = binaryReader.ReadString();
                AttackSound = binaryReader.ReadString();
                DeathSound = binaryReader.ReadString();
                Desc = binaryReader.ReadString();
                HurtedCritSound = binaryReader.ReadString();
                HurtedSound = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                PatrolRadius = binaryReader.Read7BitEncodedInt32();
                PatrolSpd = binaryReader.Read7BitEncodedInt32();
                IdleSound = binaryReader.ReadString();
                AiName = binaryReader.ReadString();
                PatrolPath = binaryReader.ReadArray<String>();
            }
        }

        return true;
    }
}

