/*
 * @Author: xiang huan
 * @Date: 2023-03-07 16:44:18
 * @Description: 技能效果修改
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SESkillEffectModifierCore.cs
 * 
 */

using UnityGameFramework.Runtime;
using System.Collections.Generic;
/// <summary>
/// 技能效果修改
/// </summary>
public class SESkillEffectModifierCore : SkillEffectBase
{
    private SkillCpt _skillCpt;

    private readonly HashSet<int> _skillIDSet = new(); //技能ID索引
    private readonly HashSet<int> _newIndexSet = new();  //新效果添加索引
    private readonly Dictionary<int, SkillEffectModifier> _effectModifierMap = new();
    public override void OnAdd()
    {
        base.OnAdd();
        _skillCpt = RefEntity.GetComponent<SkillCpt>();
        RefEntity.EntityEvent.EntitySkillAdd += OnEntitySkillUpdate;
        RefEntity.EntityEvent.EntitySkillRemove += OnEntitySkillUpdate;
    }

    public override void OnRemove()
    {
        RefEntity.EntityEvent.EntitySkillAdd -= OnEntitySkillUpdate;
        RefEntity.EntityEvent.EntitySkillRemove -= OnEntitySkillUpdate;
        CleanAllEffectModifier();
        _skillIDSet.Clear();
        _newIndexSet.Clear();
        base.OnRemove();
    }
    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }

        //技能ID,修改类型(0:替换 1:新增 2:删除), 效果作用类型（0:初始阶段 1:前摇阶段 2:对自己释放 3:对敌人释放), 效果列表;
        for (int i = 0; i < EffectCfg.Parameters2.Length; i++)
        {
            if (EffectCfg.Parameters2[i].Length < 4)
            {
                Log.Error($"SESkillAddEffectCore Parameters2 Error EffectID = {EffectID}");
                continue;
            }
            int skillID = EffectCfg.Parameters2[i][0];
            if (!_skillIDSet.Contains(skillID))
            {
                _ = _skillIDSet.Add(skillID);
            }
            _ = _newIndexSet.Add(i);
        }
        UpdateEffectModifier();
    }

    private void OnEntitySkillUpdate(int skillID)
    {
        if (!_skillIDSet.Contains(skillID))
        {
            return;
        }
        UpdateEffectModifier();
    }
    /// <summary>
    /// 更新技能效果修改器
    /// </summary>
    private void UpdateEffectModifier()
    {
        if (_skillCpt == null)
        {
            Log.Error($"SESkillAddEffectCore not find skillCpt Error EntityID = {RefEntity.BaseData.Id}");
            return;
        }
        UpdateOldEffectModifier();
        UpdateNewEffectModifier();
    }

    /// <summary>
    /// 刷新老效果修改器
    /// </summary>
    private void UpdateOldEffectModifier()
    {
        //清除无效的效果修改器
        List<int> removeIndexList = new();
        foreach (KeyValuePair<int, SkillEffectModifier> item in _effectModifierMap)
        {
            int skillID = EffectCfg.Parameters2[item.Key][0];
            if (!_skillCpt.HasSkill(skillID))
            {
                removeIndexList.Add(item.Key);
            }
        }

        for (int i = 0; i < removeIndexList.Count; i++)
        {
            _ = _newIndexSet.Add(removeIndexList[i]);
            _ = _effectModifierMap.Remove(removeIndexList[i]);
        }
    }

    /// <summary>
    /// 获取新效果索引列表
    /// </summary>
    private List<int> GetNewEffectIndexList(HashSet<int> newIndexSet)
    {
        List<int> newEffectIndexList = new();
        foreach (int index in newIndexSet)
        {
            try
            {
                int skillID = EffectCfg.Parameters2[index][0];
                if (_skillCpt.HasSkill(skillID))
                {
                    newEffectIndexList.Add(index);
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"SESkillAddEffectCore GetNewEffectIndexList Error EffectID = {EffectID} Index = {index}  Parameters2 = {EffectCfg.Parameters2} Error = {e}");
            }

        }
        return newEffectIndexList;
    }

    /// <summary>
    /// 刷新新效果修改器
    /// </summary>
    private void UpdateNewEffectModifier()
    {
        List<int> indexList = GetNewEffectIndexList(_newIndexSet);
        if (indexList.Count == 0)
        {
            return;
        }
        foreach (int index in indexList)
        {
            int skillID = EffectCfg.Parameters2[index][0];
            int modifierType = EffectCfg.Parameters2[index][1];
            int applyType = EffectCfg.Parameters2[index][2];
            int[] effectList = new int[EffectCfg.Parameters2[index].Length - 3];
            for (int i = 0; i < effectList.Length; i++)
            {
                effectList[i] = EffectCfg.Parameters2[index][i + 3];
            }
            SkillBase skill = _skillCpt.GetSkill(skillID);
            SkillEffectModifier modifier = skill.AddEffect((eSkillEffectApplyType)applyType, (eSkillEffectModifierType)modifierType, effectList);
            _effectModifierMap.Add(index, modifier);
            _ = _newIndexSet.Remove(index);
        }
    }

    /// <summary>
    /// 清除所有效果修改器
    /// </summary>
    private void CleanAllEffectModifier()
    {
        foreach (KeyValuePair<int, SkillEffectModifier> item in _effectModifierMap)
        {
            int skillID = EffectCfg.Parameters2[item.Key][0];
            SkillBase skill = _skillCpt.GetSkill(skillID);
            if (skill != null)
            {
                _ = skill.RemoveEffect(item.Value);
            }
            _ = _newIndexSet.Add(item.Key);
        }
        _effectModifierMap.Clear();
    }
}