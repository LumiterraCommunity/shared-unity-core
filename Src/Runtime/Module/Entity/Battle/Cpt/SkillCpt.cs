/*
 * @Author: xiang huan
 * @Date: 2022-07-19 13:38:00
 * @Description: 技能组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/SkillCpt.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SkillCpt : EntityBaseComponent
{
    public Dictionary<int, SkillBase> SkillMap { get; private set; } = new();
    private bool _isDestroy = false;
    private readonly List<long> _validTargetList = new();
    private readonly List<Vector3> _validTargetPosList = new();
    private void OnDestroy()
    {
        _isDestroy = true;
        RemoveAllSkill();
    }

    /// <summary>
    /// 添加技能
    /// </summary>
    public SkillBase AddSkill(int skillID)
    {
        if (_isDestroy)
        {
            return null;
        }
        if (SkillMap.ContainsKey(skillID))
        {
            Log.Warning($"Add Skill Repeat! skillId ={skillID}");
            return SkillMap[skillID];
        }
        SkillBase skill = SkillBase.Create<SkillBase>(skillID);
        SkillMap.Add(skillID, skill);
        skill.OnAdd(RefEntity);
        RefEntity.EntityEvent.EntitySkillAdd?.Invoke(skillID);
        return skill;
    }

    /// <summary>
    /// 删除技能
    /// </summary>
    public void RemoveSkill(int skillID)
    {
        if (_isDestroy)
        {
            return;
        }
        if (!SkillMap.TryGetValue(skillID, out SkillBase skill))
        {
            Log.Warning($"Remove Skill Is Null! skillId ={skillID}");
            return;
        }
        skill.OnRemove();
        skill.Dispose();
        _ = SkillMap.Remove(skillID);
        RefEntity.EntityEvent.EntitySkillRemove?.Invoke(skillID);
    }

    /// <summary>
    /// 是否已经添加包含某个技能
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public bool ContainsSkill(int skillID)
    {
        return SkillMap.ContainsKey(skillID);
    }

    public void RemoveAllSkill()
    {
        foreach (KeyValuePair<int, SkillBase> item in SkillMap)
        {
            item.Value.OnRemove();
            item.Value.Dispose();
        }
        SkillMap.Clear();
    }

    public bool CanUseSkill(int skillID, Vector3 dir, long[] targetList = null, Vector3[] targetPosList = null)
    {
        //检测是否拥有技能
        if (!SkillMap.TryGetValue(skillID, out SkillBase skill))
        {
            //TODO: home 因为现在服务器没有装备道具和技能的逻辑 但是现在只有播种和放饲料 施肥等道具技能需要用到 可以单独判断不让校验 因为没有道具即使客户端发播种技能也没用
            //先注释这个特殊逻辑
            // DRSkill dRSkill = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
            // if (dRSkill != null && dRSkill.HomeAction != null && dRSkill.HomeAction.Length > 0)
            // {
            //     HomeDefine.eAction action = dRSkill.GetHomeAction();
            //     if ((action & (HomeDefine.eAction.Sowing | HomeDefine.eAction.PutAnimalFood | HomeDefine.eAction.Manure)) != 0)
            //     {
            //         return true;
            //     }
            // }

            //道具技能不会加进技能列表，这里直接能用，释放的时候会判断道具数量
            if (!dRSkill.IsItemSkill)
            {
                Log.Warning($"CanUseSkill Skill Is Null! skillId ={skillID}");
                return false;
            }
        }

        //检测技能能否使用
        if ((skill.SkillFlag & BattleDefine.SKILL_USE_TAG) == 0)
        {
            return false;
        }

        return CheckSkillTarget(skill, targetList, targetPosList);
    }
    public bool CheckSkillTarget(SkillBase skill, long[] targetList = null, Vector3[] targetPosList = null)
    {
        //不需要目标
        if ((skill.TargetFlag & (int)eSkillTargetFlag.NotTarget) != 0)
        {
            return true;
        }

        //需要目标
        if ((skill.TargetFlag & (int)eSkillTargetFlag.Target) != 0 && targetList != null && targetList.Length > 0)
        {
            return true;

        }

        //需要位置
        if ((skill.TargetFlag & (int)eSkillTargetFlag.Pos) != 0 && targetPosList != null && targetPosList.Length > 0)
        {
            return true;
        }

        return false;
    }

    public long[] GetValidTargetList(int skillID, long[] targetList, int targetType)
    {
        _validTargetList.Clear();

        if (!SkillMap.TryGetValue(skillID, out SkillBase skill))
        {
            return _validTargetList.ToArray();
        }

        if (targetList == null || targetList.Length == 0)
        {
            return _validTargetList.ToArray();
        }

        for (int i = 0; i < targetList.Length; i++)
        {
            if (GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(targetList[i], out EntityBase targetEntity))
            {
                if (skill.IsSkillRange(targetEntity.Position) && RefEntity.EntityCampDataCore.IsSkillTarget(targetEntity, targetType))
                {
                    _validTargetList.Add(targetList[i]);
                }
            }
        }
        return _validTargetList.ToArray();
    }

    public Vector3[] GetValidTargetPosList(int skillID, Vector3[] targetPosList)
    {
        _validTargetPosList.Clear();
        if (!SkillMap.TryGetValue(skillID, out SkillBase skill))
        {
            return _validTargetPosList.ToArray();
        }

        if (targetPosList == null || targetPosList.Length == 0)
        {
            return _validTargetPosList.ToArray();
        }


        for (int i = 0; i < targetPosList.Length; i++)
        {
            if (skill.IsSkillRange(targetPosList[i]))
            {
                _validTargetPosList.Add(targetPosList[i]);
            }

        }
        return _validTargetPosList.ToArray();
    }

    /// <summary>
    /// 获得技能
    /// </summary>
    public SkillBase GetSkill(int skillID)
    {
        if (!SkillMap.TryGetValue(skillID, out SkillBase skill))
        {
            Log.Warning($"GetSkill Skill Is Null! skillId ={skillID}");
            return null;
        }
        return skill;
    }

    /// <summary>
    /// 是否拥有技能
    /// </summary>
    public bool HasSkill(int skillID)
    {
        return SkillMap.ContainsKey(skillID);
    }
}
