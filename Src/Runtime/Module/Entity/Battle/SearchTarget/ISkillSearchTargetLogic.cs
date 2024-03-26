/*
 * @Author: xiang huan
 * @Date: 2023-06-28 16:06:07
 * @Description: 技能搜索目标逻辑
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/ISkillSearchTargetLogic.cs
 * 
 */

using UnityEngine;
/// <summary>
/// 技能搜索目标逻辑
/// </summary>
public interface ISkillSearchTargetLogic
{
    int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams);
}