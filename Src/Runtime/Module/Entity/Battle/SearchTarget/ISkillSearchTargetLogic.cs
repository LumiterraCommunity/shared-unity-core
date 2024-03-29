/*
 * @Author: xiang huan
 * @Date: 2023-06-28 16:06:07
 * @Description: 技能搜索目标逻辑
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/ISkillSearchTargetLogic.cs
 * 
 */

using UnityEngine;
/// <summary>
/// 技能搜索目标逻辑
/// </summary>
public interface ISkillSearchTargetLogic
{
    /// <summary>
    /// 搜索目标
    /// </summary>
    /// <param name="searchTarget">搜索目标组件</param>
    /// <param name="dir">技能方向</param>
    /// <param name="drSkill">技能表</param>
    /// <param name="searchParams">搜索参数</param>
    /// <returns></returns>
    int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams);
}