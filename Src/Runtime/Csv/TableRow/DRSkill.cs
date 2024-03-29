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
public class DRSkill : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取IsBeHitBreakable-bool。*/
    /// </summary>
    public bool IsBeHitBreakable
    {
        get;
        private set;
    }

    /// <summary>
  /**获取IsIndependentLogic-bool。*/
    /// </summary>
    public bool IsIndependentLogic
    {
        get;
        private set;
    }

    /// <summary>
  /**获取accuBreakable-bool。*/
    /// </summary>
    public bool AccuBreakable
    {
        get;
        private set;
    }

    /// <summary>
  /**获取accuTime-int。*/
    /// </summary>
    public int AccuTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取attackCanMove-bool。*/
    /// </summary>
    public bool AttackCanMove
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectEnemy-int[]。*/
    /// </summary>
    public int[] EffectEnemy
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectForward-int[]。*/
    /// </summary>
    public int[] EffectForward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取forwardReleaseTime-int。*/
    /// </summary>
    public int ForwardReleaseTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isAutoUse-bool。*/
    /// </summary>
    public bool IsAutoUse
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isCheckHit-bool。*/
    /// </summary>
    public bool IsCheckHit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isHoldSkill-bool。*/
    /// </summary>
    public bool IsHoldSkill
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isReleaseEffFollow-bool。*/
    /// </summary>
    public bool IsReleaseEffFollow
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isReleastActLoop-bool。*/
    /// </summary>
    public bool IsReleastActLoop
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isRemote-bool。*/
    /// </summary>
    public bool IsRemote
    {
        get;
        private set;
    }

    /// <summary>
  /**获取playProgress-bool。*/
    /// </summary>
    public bool PlayProgress
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rangeTips-bool。*/
    /// </summary>
    public bool RangeTips
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseAct-string。*/
    /// </summary>
    public string ReleaseAct
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseEff-string。*/
    /// </summary>
    public string ReleaseEff
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseSound-string[]。*/
    /// </summary>
    public string[] ReleaseSound
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseSpd-int。*/
    /// </summary>
    public int ReleaseSpd
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseTime-int。*/
    /// </summary>
    public int ReleaseTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取searchTarget-int[][]。*/
    /// </summary>
    public int[][] SearchTarget
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillCD-int。*/
    /// </summary>
    public int SkillCD
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillDesc-string。*/
    /// </summary>
    public string SkillDesc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillDistance-int。*/
    /// </summary>
    public int SkillDistance
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillFlag-int[]。*/
    /// </summary>
    public int[] SkillFlag
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillFlyerId-int。*/
    /// </summary>
    public int SkillFlyerId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillName-string。*/
    /// </summary>
    public string SkillName
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillRange-int[]。*/
    /// </summary>
    public int[] SkillRange
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillShake-string[]。*/
    /// </summary>
    public string[] SkillShake
    {
        get;
        private set;
    }

    /// <summary>
  /**获取targetFlag-int[]。*/
    /// </summary>
    public int[] TargetFlag
    {
        get;
        private set;
    }

    /// <summary>
  /**获取targetLock-bool。*/
    /// </summary>
    public bool TargetLock
    {
        get;
        private set;
    }

    /// <summary>
  /**获取targetType-int[]。*/
    /// </summary>
    public int[] TargetType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取homeAction-int[]。*/
    /// </summary>
    public int[] HomeAction
    {
        get;
        private set;
    }

    /// <summary>
  /**获取homeAttRate-int。*/
    /// </summary>
    public int HomeAttRate
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectSelf-int[]。*/
    /// </summary>
    public int[] EffectSelf
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillIcon-string。*/
    /// </summary>
    public string SkillIcon
    {
        get;
        private set;
    }

    /// <summary>
  /**获取hitEff-string。*/
    /// </summary>
    public string HitEff
    {
        get;
        private set;
    }

    /// <summary>
  /**获取animRotate-int。*/
    /// </summary>
    public int AnimRotate
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillAlertEffect-string。*/
    /// </summary>
    public string SkillAlertEffect
    {
        get;
        private set;
    }

    /// <summary>
  /**获取skillFlyerNum-int。*/
    /// </summary>
    public int SkillFlyerNum
    {
        get;
        private set;
    }

    /// <summary>
  /**获取accuEff-int。*/
    /// </summary>
    public int AccuEff
    {
        get;
        private set;
    }

    /// <summary>
  /**获取effectInit-int[]。*/
    /// </summary>
    public int[] EffectInit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取composeSkill-int[]。*/
    /// </summary>
    public int[] ComposeSkill
    {
        get;
        private set;
    }

    /// <summary>
  /**获取costPropType-int[]。*/
    /// </summary>
    public int[] CostPropType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取maxAccuTime-int。*/
    /// </summary>
    public int MaxAccuTime
    {
        get;
        private set;
    }

    /// <summary>
  /**获取itemCost-int[][]。*/
    /// </summary>
    public int[][] ItemCost
    {
        get;
        private set;
    }

    /// <summary>
  /**获取accuTab-string。*/
    /// </summary>
    public string AccuTab
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        IsBeHitBreakable = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsIndependentLogic = DataTableParseUtil.ParseBool(columnStrings[index++]);
        AccuBreakable = DataTableParseUtil.ParseBool(columnStrings[index++]);
        AccuTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        AttackCanMove = DataTableParseUtil.ParseBool(columnStrings[index++]);
        EffectEnemy = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        EffectForward = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ForwardReleaseTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsAutoUse = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsCheckHit = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsHoldSkill = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsReleaseEffFollow = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsReleastActLoop = DataTableParseUtil.ParseBool(columnStrings[index++]);
        IsRemote = DataTableParseUtil.ParseBool(columnStrings[index++]);
        PlayProgress = DataTableParseUtil.ParseBool(columnStrings[index++]);
        RangeTips = DataTableParseUtil.ParseBool(columnStrings[index++]);
        ReleaseAct = DataTableParseUtil.ParseString(columnStrings[index++]);
        ReleaseEff = DataTableParseUtil.ParseString(columnStrings[index++]);
        ReleaseSound = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        ReleaseSpd = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ReleaseTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SearchTarget = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        SkillCD = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SkillDesc = DataTableParseUtil.ParseString(columnStrings[index++]);
        SkillDistance = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SkillFlag = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        SkillFlyerId = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SkillName = DataTableParseUtil.ParseString(columnStrings[index++]);
        SkillRange = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        SkillShake = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        TargetFlag = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        TargetLock = DataTableParseUtil.ParseBool(columnStrings[index++]);
        TargetType = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        HomeAction = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        HomeAttRate = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EffectSelf = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        SkillIcon = DataTableParseUtil.ParseString(columnStrings[index++]);
        HitEff = DataTableParseUtil.ParseString(columnStrings[index++]);
        AnimRotate = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SkillAlertEffect = DataTableParseUtil.ParseString(columnStrings[index++]);
        SkillFlyerNum = DataTableParseUtil.ParseInt(columnStrings[index++]);
        AccuEff = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EffectInit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ComposeSkill = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        CostPropType = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        MaxAccuTime = DataTableParseUtil.ParseInt(columnStrings[index++]);
        ItemCost = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        AccuTab = DataTableParseUtil.ParseString(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                IsBeHitBreakable = binaryReader.ReadBoolean();
                IsIndependentLogic = binaryReader.ReadBoolean();
                AccuBreakable = binaryReader.ReadBoolean();
                AccuTime = binaryReader.Read7BitEncodedInt32();
                AttackCanMove = binaryReader.ReadBoolean();
                EffectEnemy = binaryReader.ReadArray<Int32>();
                EffectForward = binaryReader.ReadArray<Int32>();
                ForwardReleaseTime = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                IsAutoUse = binaryReader.ReadBoolean();
                IsCheckHit = binaryReader.ReadBoolean();
                IsHoldSkill = binaryReader.ReadBoolean();
                IsReleaseEffFollow = binaryReader.ReadBoolean();
                IsReleastActLoop = binaryReader.ReadBoolean();
                IsRemote = binaryReader.ReadBoolean();
                PlayProgress = binaryReader.ReadBoolean();
                RangeTips = binaryReader.ReadBoolean();
                ReleaseAct = binaryReader.ReadString();
                ReleaseEff = binaryReader.ReadString();
                ReleaseSound = binaryReader.ReadArray<String>();
                ReleaseSpd = binaryReader.Read7BitEncodedInt32();
                ReleaseTime = binaryReader.Read7BitEncodedInt32();
                SearchTarget = binaryReader.ReadArrayList<Int32>();
                SkillCD = binaryReader.Read7BitEncodedInt32();
                SkillDesc = binaryReader.ReadString();
                SkillDistance = binaryReader.Read7BitEncodedInt32();
                SkillFlag = binaryReader.ReadArray<Int32>();
                SkillFlyerId = binaryReader.Read7BitEncodedInt32();
                SkillName = binaryReader.ReadString();
                SkillRange = binaryReader.ReadArray<Int32>();
                SkillShake = binaryReader.ReadArray<String>();
                TargetFlag = binaryReader.ReadArray<Int32>();
                TargetLock = binaryReader.ReadBoolean();
                TargetType = binaryReader.ReadArray<Int32>();
                HomeAction = binaryReader.ReadArray<Int32>();
                HomeAttRate = binaryReader.Read7BitEncodedInt32();
                EffectSelf = binaryReader.ReadArray<Int32>();
                SkillIcon = binaryReader.ReadString();
                HitEff = binaryReader.ReadString();
                AnimRotate = binaryReader.Read7BitEncodedInt32();
                SkillAlertEffect = binaryReader.ReadString();
                SkillFlyerNum = binaryReader.Read7BitEncodedInt32();
                AccuEff = binaryReader.Read7BitEncodedInt32();
                EffectInit = binaryReader.ReadArray<Int32>();
                ComposeSkill = binaryReader.ReadArray<Int32>();
                CostPropType = binaryReader.ReadArray<Int32>();
                MaxAccuTime = binaryReader.Read7BitEncodedInt32();
                ItemCost = binaryReader.ReadArrayList<Int32>();
                AccuTab = binaryReader.ReadString();
            }
        }

        return true;
    }
}
