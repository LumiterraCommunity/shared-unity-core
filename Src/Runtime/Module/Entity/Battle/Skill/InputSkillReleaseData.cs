/*
 * @Author: xiang huan
 * @Date: 2023-02-09 14:54:09
 * @Description: 技能输入数据 
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Skill/InputSkillReleaseData.cs
 * 
 */


using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public class InputSkillReleaseData
{
    /// <summary>
    /// 技能位置
    /// </summary>
    public int SkillID { get; private set; }
    /// <summary>
    /// 技能配置
    /// </summary>
    public DRSkill DRSkill { get; private set; }
    /// <summary>
    /// 技能目标类型
    /// </summary>
    public int TargetType { get; private set; }
    /// <summary>
    /// 技能飞行物配置
    /// </summary>
    public DRSkillFlyer DRSkillFlyer { get; private set; }
    /// <summary>
    /// 输入技能的位置 如果是玩家输入技能代表客户端请求的输入位置
    /// </summary>
    /// <value></value>
    public Vector3 Pos { get; private set; }
    /// <summary>
    /// 目标方向
    /// </summary>
    public Vector3 Dir { get; private set; }
    /// <summary>
    /// 目标列表
    /// </summary>
    public long[] Targets { get; private set; }
    /// <summary>
    /// 目标位置
    /// </summary>
    public Vector3[] TargetPosList { get; private set; }

    /// <summary>
    /// 技能消耗道具
    /// </summary>
    public GameMessageCore.UseSkillCostItem CostItem { get; private set; }
    /// <summary>
    /// 技能释放速率
    /// </summary>
    public double SkillTimeScale { get; private set; }

    /// <summary>
    /// 技能蓄力时间 s
    /// </summary>
    public float AccumulateTime { get; private set; }

    /// <summary>
    /// 输入随机数
    /// </summary>
    public InputRandomData InputRandom { get; private set; }

    /// <summary>
    /// 是否为预释放
    /// </summary>
    public bool IsPreRelease { get; private set; }

    /// <summary>
    /// 家园攻击目标
    /// </summary>
    /// <value></value>
    public ICollectResourceCore[] HomeTargets { get; private set; }

    /// <summary>
    /// 技能输入数据
    /// </summary>
    /// <param name="skillID">技能ID</param>
    /// <param name="dir">技能方向</param>
    /// <param name="pos">输入技能的位置</param>
    /// <param name="targets">技能目标列表</param>
    /// <param name="targetPosList">技能目标位置列表</param>
    /// <param name="skillTimeScale">释放速率</param>
    public InputSkillReleaseData(int skillID, Vector3 dir, Vector3 pos, long[] targets = null, Vector3[] targetPosList = null, double skillTimeScale = 1, GameMessageCore.UseSkillCostItem costItem = null, float accumulateTime = 0)
    {
        SkillID = skillID;
        Pos = pos;
        Dir = dir;
        Targets = targets;
        TargetPosList = targetPosList;
        SkillTimeScale = skillTimeScale;
        CostItem = costItem;
        AccumulateTime = accumulateTime;
        DRSkill = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (DRSkill == null)
        {
            Log.Error("InputSkillReleaseData The skill ID was not found in the skill table:{0}", skillID);
            return;
        }
        TargetType = SkillUtil.GetFlag(DRSkill.TargetType);
        DRSkillFlyer = GFEntryCore.DataTable.GetDataTable<DRSkillFlyer>().GetDataRow(DRSkill.SkillFlyerId);
        IsPreRelease = false;
    }

    public void SetInputRandomSeed(int seed)
    {
        InputRandom ??= new InputRandomData();
        InputRandom.SetInputRandomSeed(seed);
    }

    public void SetHomeTargets(ICollectResourceCore[] homeTargets)
    {
        HomeTargets = homeTargets;
    }

    public void SetIsPreRelease(bool isPreRelease)
    {
        IsPreRelease = isPreRelease;
    }
}