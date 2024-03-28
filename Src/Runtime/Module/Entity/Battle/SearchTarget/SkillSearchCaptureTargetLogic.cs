/*
 * @Author: xiang huan
 * @Date: 2022-10-14 15:35:49
 * @Description: 技能搜索捕获目标逻辑
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SearchTarget/SkillSearchCaptureTargetLogic.cs
 * 
 */

using System.Collections.Generic;
using UnityEngine;
public class SkillSearchCaptureTargetLogic : ISkillSearchTargetLogic
{
    public int SearchTarget(EntitySkillSearchTarget searchTarget, Vector3 dir, DRSkill drSkill, int[] searchParams)
    {
        EntityBase entity = searchTarget.RefEntity;

        long captureId = entity.CaptureData.CaptureId;
        if (GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(captureId, out EntityBase captureEntity))
        {
            searchTarget.AddTarget(new List<EntityBase> { captureEntity });
            return 1;
        }
        return 0;
    }
}