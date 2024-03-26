/*
 * @Author: xiang huan
 * @Date: 2022-10-14 15:35:49
 * @Description: 技能范围搜索目标逻辑
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/SkillSearchRangeTargetLogic.cs
 * 
 */

using System.Collections.Generic;
using UnityEngine;

public class SkillSearchRangeTargetLogic : ISkillSearchTargetLogic
{
    public int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams)
    {
        EntityBase entity = searchTarget.RefEntity;
        int skillTargetType = SkillUtil.GetFlag(drSkill.TargetType);
        if (drSkill.SkillRange == null || drSkill.SkillRange.Length == 0)
        {
            return 0;
        }

        List<EntityBase> searchList = SearchSkillRangeTarget(entity, drSkill, skillTargetType, dir);
        if (searchList.Count < searchTarget.TargetNum)
        {
            searchList = SearchSkillDistanceTarget(entity, drSkill, skillTargetType, dir);
        }

        //根据距离排序 由近到远
        searchList.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(a.Position, entity.Position);
            float distanceB = Vector3.Distance(b.Position, entity.Position);
            return distanceA.CompareTo(distanceB);
        });

        List<EntityBase> targetList = new();
        List<Vector3> posList = new();
        for (int i = 0; i < searchList.Count && i < searchTarget.TargetNum; i++)
        {
            targetList.Add(searchList[i]);
            posList.Add(searchList[i].Position);
        }
        //更新目标
        searchTarget.UpdateTarget(targetList);
        searchTarget.UpdateTargetPos(posList);

        //更新方向
        if (targetList.Count > 0)
        {
            dir = (targetList[0].Position - entity.Position).normalized;
            searchTarget.UpdateTargetDir(dir);
        }
        return targetList.Count;
    }

    protected List<EntityBase> SearchSkillRangeTarget(EntityBase entity, DRSkill drSkill, int skillTargetType, Vector3 dir)
    {
        List<EntityBase> searchEntities = SkillUtil.SearchTargetEntityList(entity.RoleBaseDataCore.CenterPos, entity, drSkill.SkillRange, dir, skillTargetType);
        List<EntityBase> targetEntities = UpdateSearchTarget(searchEntities);
        return targetEntities;
    }

    protected List<EntityBase> SearchSkillDistanceTarget(EntityBase entity, DRSkill drSkill, int skillTargetType, Vector3 dir)
    {
        int[] range = { (int)BattleDefine.eSkillShapeId.SkillShapeSphere, drSkill.SkillDistance };
        List<EntityBase> searchEntities = SkillUtil.SearchTargetEntityList(entity.RoleBaseDataCore.CenterPos, entity, range, dir, skillTargetType);
        List<EntityBase> targetEntities = UpdateSearchTarget(searchEntities);
        return targetEntities;
    }

    /// <summary>
    /// 筛选目标
    /// </summary>
    /// <param name="searchList"></param>
    /// <returns></returns>
    protected List<EntityBase> UpdateSearchTarget(List<EntityBase> searchList)
    {
        List<EntityBase> targetList = new();
        if (searchList.Count > 0)
        {
            for (int i = 0; i < searchList.Count; i++)
            {
                EntityBase searchEntity = searchList[i];
                if (searchEntity.Inited && searchEntity.BattleDataCore != null && searchEntity.BattleDataCore.IsLive())
                {
                    targetList.Add(searchEntity);
                }
            }
        }
        return targetList;
    }
}