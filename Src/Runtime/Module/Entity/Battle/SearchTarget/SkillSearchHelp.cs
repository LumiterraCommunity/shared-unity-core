/*
 * @Author: xiang huan
 * @Date: 2023-06-28 17:04:48
 * @Description: 技能搜索帮助类
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/SkillSearchHelp.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public static class SkillSearchHelp
{
    private static readonly Dictionary<eSearchTargetType, ISkillSearchTargetLogic> s_searchLogicDic = new()
    {
        {eSearchTargetType.RangeTarget, new SkillSearchRangeTargetLogic()},
        {eSearchTargetType.RangeRandomPos, new SkillSearchRandomPosLogic()},
        {eSearchTargetType.CaptureTarget, new SkillSearchCaptureTargetLogic()},
    };

    public static int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams)
    {
        if (s_searchLogicDic.TryGetValue((eSearchTargetType)searchParams[0], out ISkillSearchTargetLogic logic))
        {
            return logic.SearchTarget(searchTarget, dir, drSkill, searchParams);
        }
        else
        {
            Log.Error($"SearchTargetType not found {searchParams[0]}");
        }
        return 0;
    }

}