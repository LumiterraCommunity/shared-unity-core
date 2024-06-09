/*
 * @Author: xiang huan
 * @Date: 2023-01-06 10:52:11
 * @Description:  技能基础, 用了引用池，记住继承Clear清除数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Skill/SkillBase.cs
 * 
 */
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

public class SkillBase : IReference
{
    public int SkillID { get; private set; }
    public DRSkill DRSkill { get; private set; }
    /// <summary>
    ///技能标识
    /// </summary>
    public int SkillFlag { get; private set; }
    /// <summary>
    ///技能释放目标标识
    /// </summary>
    public int TargetFlag { get; private set; }

    /// <summary>
    ///技能释放目标类型
    /// </summary>
    public int TargetType { get; private set; }

    /// <summary>
    /// 宿主对象
    /// </summary>
    public EntityBase RefEntity { get; private set; }

    /// <summary>
    /// 效果Map
    /// </summary>
    public Dictionary<eSkillEffectApplyType, List<int>> EffectMap { get; private set; } = new();

    /// <summary>
    /// 效果修改器Map
    /// </summary>
    public Dictionary<eSkillEffectApplyType, List<SkillEffectModifier>> EffectModifierMap { get; private set; } = new();

    /// <summary>
    /// 是否添加
    /// </summary>
    public bool IsAdd { get; private set; }
    public void SetData(int skillID)
    {
        SkillID = skillID;
        DRSkill = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (DRSkill == null)
        {
            Log.Error("The skill ID was not found in the skill table:{0}", skillID);
            return;

        }
        SkillFlag = 0;
        if (DRSkill.SkillFlag != null)
        {
            for (int i = 0; i < DRSkill.SkillFlag.Length; i++)
            {
                SkillFlag |= 1 << DRSkill.SkillFlag[i];
            }
        }
        TargetFlag = 0;
        if (DRSkill.TargetFlag != null)
        {
            for (int i = 0; i < DRSkill.TargetFlag.Length; i++)
            {
                TargetFlag |= 1 << DRSkill.TargetFlag[i];
            }
        }

        TargetType = 0;
        if (DRSkill.TargetType != null)
        {
            for (int i = 0; i < DRSkill.TargetType.Length; i++)
            {
                TargetType |= 1 << DRSkill.TargetType[i];
            }
        }
        //添加效果
        _ = AddEffect(eSkillEffectApplyType.Init, eSkillEffectModifierType.Add, DRSkill.EffectInit);
        _ = AddEffect(eSkillEffectApplyType.Forward, eSkillEffectModifierType.Add, DRSkill.EffectForward);
        _ = AddEffect(eSkillEffectApplyType.CastSelf, eSkillEffectModifierType.Add, DRSkill.EffectSelf);
        _ = AddEffect(eSkillEffectApplyType.CastEnemy, eSkillEffectModifierType.Add, DRSkill.EffectEnemy);
    }

    /// <summary>
    /// 添加效果
    /// </summary>
    /// <param name="applyType"></param>
    /// <param name="type"></param>
    /// <param name="effectList"></param>
    public SkillEffectModifier AddEffect(eSkillEffectApplyType applyType, eSkillEffectModifierType type, int[] effectList)
    {
        if (effectList == null)
        {
            return new();
        }

        if (!EffectModifierMap.TryGetValue(applyType, out List<SkillEffectModifier> list))
        {
            list = new();
            EffectModifierMap.Add(applyType, list);
        }
        SkillEffectModifier modifier = SkillEffectModifier.Create(applyType, type, effectList);
        list.Add(modifier);
        UpdateEffectList(applyType);
        return modifier;
    }

    public int[] GetEffect(eSkillEffectApplyType type)
    {
        return !EffectMap.TryGetValue(type, out List<int> list) ? null : list.ToArray();
    }

    /// <summary>
    /// 删除效果
    /// </summary>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public bool RemoveEffect(SkillEffectModifier modifier)
    {

        if (!EffectModifierMap.TryGetValue(modifier.ApplyType, out List<SkillEffectModifier> modifierList))
        {
            return false;
        }

        if (modifierList.Remove(modifier))
        {
            UpdateEffectList(modifier.ApplyType);
            modifier.Dispose();
        }
        return true;
    }

    /// <summary>
    /// 清除所有效果
    /// </summary>
    /// <returns></returns>
    public bool CleanAllEffect()
    {
        foreach (KeyValuePair<eSkillEffectApplyType, List<SkillEffectModifier>> item in EffectModifierMap)
        {
            foreach (SkillEffectModifier modifier in item.Value)
            {
                modifier.Dispose();
            }
        }
        EffectModifierMap.Clear();
        EffectMap.Clear();
        return true;
    }
    /// <summary>
    /// 更新效果列表
    /// </summary>
    public void UpdateEffectList(eSkillEffectApplyType type)
    {
        if (!EffectModifierMap.TryGetValue(type, out List<SkillEffectModifier> modifierList))
        {
            return;
        }
        if (!EffectMap.TryGetValue(type, out List<int> oldList))
        {
            oldList = new();
            EffectMap.Add(type, oldList);
        }
        List<int> newList = SkillUtil.CalculateSkillEffectModifierList(modifierList);
        EffectMap[type] = newList;
        if (type == eSkillEffectApplyType.Init)
        {
            UpdateInitEffect(newList.ToArray(), oldList.ToArray());
        }
    }

    /// <summary>
    /// 更新初始化效果
    /// </summary>
    public void UpdateInitEffect(int[] newList, int[] oldList)
    {
        if (!IsAdd)
        {
            return;
        }

        if (oldList != null && oldList.Length > 0)
        {
            SkillUtil.EntityAbolishSkillEffect(SkillID, oldList, RefEntity, RefEntity);
        }

        if (newList != null && newList.Length > 0)
        {
            InputSkillReleaseData inputData = new(SkillID, Vector3.zero, RefEntity.Position);
            _ = SkillUtil.EntitySkillEffectExecute(inputData, newList, RefEntity, RefEntity);
        }

    }
    /// <summary>
    /// 技能被添加
    /// </summary>
    public virtual void OnAdd(EntityBase owner)
    {
        IsAdd = true;
        RefEntity = owner;
        UpdateInitEffect(GetEffect(eSkillEffectApplyType.Init), null);
    }

    /// <summary>
    /// 技能被移除
    /// </summary>
    public virtual void OnRemove()
    {
        UpdateInitEffect(null, GetEffect(eSkillEffectApplyType.Init));
        _ = CleanAllEffect();
        RefEntity = null;
        IsAdd = false;
    }

    /// <summary>
    /// 技能开启
    /// </summary>
    public virtual void OnToggleOn()
    {

    }

    /// <summary>
    /// 技能关闭
    /// </summary>
    public virtual void OnToggleOff()
    {

    }
    public virtual void Clear()
    {
        SkillID = 0;
        DRSkill = null;
        RefEntity = null;
        EffectMap.Clear();
    }

    /// <summary>
    /// 是否在技能范围内
    /// </summary>
    public bool IsSkillRange(Vector3 pos)
    {
        if (DRSkill == null)
        {
            return false;
        }

        if (DRSkill.SkillDistance <= 0)//没配置范围 代表一些没有距离要求的特殊技能
        {
            return true;
        }

        float distance = Vector3.Distance(RefEntity.Position, pos);
        return distance <= DRSkill.SkillDistance * MathUtilCore.CM2M;
    }

    /// <summary>
    /// 创建技能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Create<T>(int skillID) where T : SkillBase, new()
    {
        T skill = ReferencePool.Acquire<T>();
        skill.SetData(skillID);
        return skill;
    }

    /// <summary>
    /// 效果被销毁
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }
}