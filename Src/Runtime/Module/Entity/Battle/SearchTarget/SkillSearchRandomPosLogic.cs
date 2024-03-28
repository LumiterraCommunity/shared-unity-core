/*
 * @Author: xiang huan
 * @Date: 2022-10-14 15:35:49
 * @Description: 技能搜索随机位置逻辑
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/SkillSearchRandomPosLogic.cs
 * 
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SkillSearchRandomPosLogic : ISkillSearchTargetLogic
{
    public int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams)
    {
        EntityBase entity = searchTarget.RefEntity;
        List<Vector3> posList = new();
        int searchNum = 0;
        for (int i = searchTarget.SearchNum; i < searchTarget.TargetNum; i++)
        {
            Vector2 rnd = drSkill.SkillDistance * MathUtilCore.CM2M * UnityEngine.Random.insideUnitCircle;
            Vector3 randPos = entity.Position + new Vector3(rnd.x, 0, rnd.y);
            _ = MapUtilCore.SamplePosOnTerrain(randPos, out Vector3 pos, 5f);
            posList.Add(pos);
            searchNum++;
        }
        searchTarget.AddTargetPos(posList);
        return searchNum;
    }
}