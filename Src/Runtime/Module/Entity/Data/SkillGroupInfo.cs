/*
 * @Author: xiang huan
 * @Date: 2023-11-21 15:53:33
 * @Description: 技能组数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/SkillGroupInfo.cs
 * 
 */
public class SkillGroupInfo
{
    public eSkillGroupType SkillGroupType; //技能数据类型
    public readonly ListMap<int, int> SkillIDList = new();
    private readonly SkillCpt _skillCpt;             //技能组件
    private int[] _skillIDArray;
    public int[] SkillIDArray               //技能数组缓存
    {
        get
        {
            if (_skillIDArray == null)
            {
                _skillIDArray = SkillIDList.ToArray();
            }
            return _skillIDArray;
        }
    }
    public SkillGroupInfo(eSkillGroupType skillGroupType, SkillCpt skillCpt)
    {
        SkillGroupType = skillGroupType;
        _skillCpt = skillCpt;
    }

    public bool HasSkillID(int skillID)
    {
        return SkillIDList.ContainsKey(skillID);
    }

    public void AddSkillID(int skillID)
    {
        if (SkillIDList.ContainsKey(skillID))
        {
            return;
        }
        _ = SkillIDList.Add(skillID, skillID);
        _ = _skillCpt.AddSkill(skillID);
        UpdateSkillIDArray();
    }

    public void RemoveSkillID(int skillID)
    {
        if (!SkillIDList.ContainsKey(skillID))
        {
            return;
        }
        _ = SkillIDList.Remove(skillID);
        _skillCpt.RemoveSkill(skillID);
        UpdateSkillIDArray();
    }

    public void SetSkillIDList(int[] skillIDs)
    {
        CleanSkillIDList();
        for (int i = 0; i < skillIDs.Length; i++)
        {
            AddSkillID(skillIDs[i]);
        }
        UpdateSkillIDArray();
    }

    public void CleanSkillIDList()
    {
        for (int i = 0; i < SkillIDList.Count; i++)
        {
            _skillCpt.RemoveSkill(SkillIDList[i]);
        }
        SkillIDList.Clear();
        UpdateSkillIDArray();
    }

    private void UpdateSkillIDArray()
    {
        _skillIDArray = SkillIDList.ToArray();
    }

}