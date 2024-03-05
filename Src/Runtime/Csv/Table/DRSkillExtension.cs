
using System.Collections.Generic;

/// <summary>
/// 技能表记录数据的扩展 有些解析比较耗性能 需要缓存 比如转家园动作需要遍历
/// </summary>
public static class DRSkillExtension
{
    private static Dictionary<int, HomeDefine.eAction> _homeActionMap;

    /// <summary>
    /// 获取表中对应的家园动作 已经完成了解析
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static HomeDefine.eAction GetHomeAction(this DRSkill skill)
    {
        _homeActionMap ??= new Dictionary<int, HomeDefine.eAction>();

        if (_homeActionMap.TryGetValue(skill.Id, out HomeDefine.eAction action))
        {
            return action;
        }
        else
        {
            HomeDefine.eAction homeAction = TableUtil.ToHomeAction(skill.HomeAction);
            _homeActionMap.Add(skill.Id, homeAction);
            return homeAction;
        }
    }
}