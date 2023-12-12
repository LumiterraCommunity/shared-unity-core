/*
 * @Author: xiang huan
 * @Date: 2022-07-29 10:08:50
 * @Description: 实体技能搜索目标
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/EntitySkillSearchTarget.cs
 * 
 */


using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EntitySkillSearchTarget : EntityBaseComponent
{
    public List<EntityBase> TargetList { get; protected set; } = new();//目标实体

    public int TargetNum { get; protected set; } = 1;//目标数量
    protected EntityInputData InputData;
    protected virtual void Start()
    {
        InputData = GetComponent<EntityInputData>();
    }
    private void OnDestroy()
    {
        UpdateTarget(null);
    }
    public virtual void UpdateTarget(List<EntityBase> targetEntities)
    {
        TargetList.Clear();

        if (targetEntities == null || targetEntities.Count <= 0)
        {
            return;
        }

        foreach (EntityBase entity in targetEntities)
        {
            if (!entity.Inited || entity.BattleDataCore == null || !entity.BattleDataCore.IsLive())
            {
                continue;
            }
            TargetList.Add(entity);
        }

        //根据距离排序 由近到远
        TargetList.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(a.Position, RefEntity.Position);
            float distanceB = Vector3.Distance(b.Position, RefEntity.Position);
            return distanceA.CompareTo(distanceB);
        });

        //只保留目标数量
        if (TargetList.Count > TargetNum)
        {
            TargetList.RemoveRange(TargetNum, TargetList.Count - TargetNum);
        }
    }

    public virtual void SearchTarget(int skillID, int targetNum = 1)
    {
        UpdateTarget(null);
        DRSkill drSkill = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (drSkill == null)
        {
            Log.Error($"not find skill id:{skillID}");
            return;
        }
        int skillTargetType = SkillUtil.GetFlag(drSkill.TargetType);
        TargetNum = targetNum;
        List<EntityBase> targetEntities = SearchSkillRangeTarget(drSkill, skillTargetType);
        if (targetEntities.Count < TargetNum)
        {
            targetEntities = SearchSkillDistanceTarget(drSkill, skillTargetType);
        }
        UpdateTarget(targetEntities);
    }

    protected List<EntityBase> SearchSkillRangeTarget(DRSkill drSkill, int skillTargetType)
    {
        if (drSkill.SkillRange == null || drSkill.SkillRange.Length == 0)
        {
            return new();
        }

        Vector3 dir = RefEntity.Forward;
        if (InputData != null && InputData.InputMoveDirection != null)//有输入移动方向需要按照输入方向
        {
            dir = InputData.InputMoveDirection.Value;
        }
        List<EntityBase> targetEntities = SkillUtil.SearchTargetEntityList(RefEntity.RoleBaseDataCore.CenterPos, RefEntity, drSkill.SkillRange, dir, skillTargetType);
        return targetEntities;
    }

    protected List<EntityBase> SearchSkillDistanceTarget(DRSkill drSkill, int skillTargetType)
    {
        int[] range = { (int)BattleDefine.eSkillShapeId.SkillShapeSphere, drSkill.SkillDistance };
        List<EntityBase> targetEntities = SkillUtil.SearchTargetEntityList(RefEntity.RoleBaseDataCore.CenterPos, RefEntity, range, RefEntity.Forward, skillTargetType);
        return targetEntities;
    }
}
