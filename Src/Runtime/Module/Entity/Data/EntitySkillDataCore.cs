/*
 * @Author: xiang huan
 * @Date: 2022-08-09 14:10:48
 * @Description: 实体技能数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/EntitySkillDataCore.cs
 * 
 */
using System.Collections.Generic;
using GameMessageCore;
public class EntitySkillDataCore : EntityBaseComponent
{
    public bool IsNetDirty = true;
    private readonly List<SkillGroupData> _netDataList = new();
    public Dictionary<eSkillGroupType, SkillGroupInfo> SkillGroupInfoDic = new();
    public eSkillGroupType[] SYNC_SKILL_GROUP_TYPE_ARRAY = { eSkillGroupType.Talent, eSkillGroupType.Extend };
    private SkillCpt _skillCpt;
    protected SkillCpt SkillComponent
    {
        get
        {
            if (_skillCpt == null)
            {
                _skillCpt = gameObject.GetComponent<SkillCpt>();
            }
            return _skillCpt;
        }
    }

    /// <summary>
    /// 连击技能组
    /// </summary>
    /// <returns></returns>
    public SkillGroupInfo ComboSkillGroupInfo
    {
        get
        {
            if (SkillGroupInfoDic.TryGetValue(eSkillGroupType.Equipment, out SkillGroupInfo skillGroupInfo) && skillGroupInfo.SkillIDArray.Length > 0)
            {
                return skillGroupInfo;
            }

            if (SkillGroupInfoDic.TryGetValue(eSkillGroupType.Base, out skillGroupInfo) && skillGroupInfo.SkillIDArray.Length > 0)
            {
                return skillGroupInfo;
            }
            return null;
        }
    }

    /// <summary>
    /// 连击技能组ID列表
    /// </summary>
    /// <returns></returns>
    public int[] ComboSkillGroupIDList => ComboSkillGroupInfo?.SkillIDArray;

    protected virtual void OnDestroy()
    {
        CleanSkillGroupDic();
    }
    /// <summary>
    /// 获得技能组信息
    /// </summary>
    /// <param name="skillGroupType"></param>
    /// <returns></returns>
    public SkillGroupInfo GetSkillGroupInfo(eSkillGroupType skillGroupType)
    {
        return SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillGroupInfo) ? skillGroupInfo : null;
    }
    /// <summary>
    /// 获得技能组ID列表
    /// </summary>
    /// <param name="skillGroupType"></param>
    /// <returns></returns>
    public int[] GetSkillGroupIDList(eSkillGroupType skillGroupType)
    {
        return SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillGroupInfo) ? skillGroupInfo.SkillIDArray : null;
    }

    /// <summary>
    /// 设置技能组ID列表
    /// </summary>
    /// <param name="skillGroupType"></param>
    /// <param name="skillIDs"></param>
    /// <returns></returns>
    public virtual void SetSkillGroupIDList(eSkillGroupType skillGroupType, int[] skillIDs)
    {
        if (!SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillDataInfo))
        {
            skillDataInfo = new(skillGroupType, SkillComponent);
            SkillGroupInfoDic.Add(skillGroupType, skillDataInfo);
        }
        skillDataInfo.SetSkillIDList(skillIDs);
        IsNetDirty = true;
    }

    /// <summary>
    /// 移除技能组ID列表
    /// </summary>
    /// <param name="skillGroupType"></param>//  
    public void RemoveSkillGroupIDList(eSkillGroupType skillGroupType)
    {
        if (SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillDataInfo))
        {
            skillDataInfo.CleanSkillIDList();
            _ = SkillGroupInfoDic.Remove(skillGroupType);
        }
        IsNetDirty = true;
    }

    /// <summary>
    /// 添加技能组ID
    /// </summary>
    /// <param name="skillGroupType"></param>
    /// <param name="skillID"></param>
    public void AddSkillGroupID(eSkillGroupType skillGroupType, int skillID)
    {
        if (!SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillDataInfo))
        {
            skillDataInfo = new(skillGroupType, SkillComponent);
            SkillGroupInfoDic.Add(skillGroupType, skillDataInfo);
        }
        skillDataInfo.AddSkillID(skillID);
        IsNetDirty = true;
    }

    /// <summary>
    /// 删除技能组ID
    /// </summary>
    /// <param name="skillGroupType"></param>
    /// <param name="skillID"></param>
    public void RemoveSkillGroupID(eSkillGroupType skillGroupType, int skillID)
    {
        if (SkillGroupInfoDic.TryGetValue(skillGroupType, out SkillGroupInfo skillDataInfo))
        {
            skillDataInfo.RemoveSkillID(skillID);
        }
        IsNetDirty = true;
    }

    /// <summary>
    /// 清除所有技能组ID
    /// </summary>
    public void CleanSkillGroupDic()
    {
        foreach (KeyValuePair<eSkillGroupType, SkillGroupInfo> keyValuePair in SkillGroupInfoDic)
        {
            keyValuePair.Value.CleanSkillIDList();
        }
        SkillGroupInfoDic.Clear();
        IsNetDirty = true;
    }

    /// <summary>
    /// 某个技能是否属于连击技能组的 往往也是代表了普通攻击
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public bool IsComboGroupSkill(int skillID)
    {
        if (ComboSkillGroupInfo != null)
        {
            return ComboSkillGroupInfo.HasSkillID(skillID);
        }
        return false;
    }

    //广播更新技能组 子类实现
    public virtual void BroadCastUpdateSkillGroup(eSkillGroupType skillGroupType)
    {

    }
    public List<SkillGroupData> GetNetData()
    {
        if (IsNetDirty)
        {
            _netDataList.Clear();
            for (int i = 0; i < SYNC_SKILL_GROUP_TYPE_ARRAY.Length; i++)
            {
                if (SkillGroupInfoDic.TryGetValue(SYNC_SKILL_GROUP_TYPE_ARRAY[i], out SkillGroupInfo skillGroupInfo) && skillGroupInfo.SkillIDArray.Length > 0)
                {
                    SkillGroupData skillGroupData = new();
                    skillGroupData.GroupType = (int)skillGroupInfo.SkillGroupType;
                    skillGroupData.SkillIdList.AddRange(skillGroupInfo.SkillIDArray);
                    _netDataList.Add(skillGroupData);
                }
            }
            IsNetDirty = false;
        }
        return _netDataList;
    }
}