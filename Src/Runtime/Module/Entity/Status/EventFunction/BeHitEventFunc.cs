using UnityGameFramework.Runtime;
/// <summary>
/// 受击事件
/// </summary>
public class BeHitEventFunc : EntityStatusEventFunctionBase
{
    public override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeHit += OnEntityBeHit;
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeHit -= OnEntityBeHit;
    }

    private void OnEntityBeHit(int damage)
    {
        // 技能阶段 需要判断 是否被打中断技能
        if (EntityStatus.StatusName == SkillAccumulateStatusCore.Name
            || EntityStatus.StatusName == SkillForwardStatusCore.Name
            || EntityStatus.StatusName == SkillCastStatusCore.Name)
        {
            InputSkillReleaseData inputSkill = OwnerFsm.GetData<VarInputSkill>(StatusDataDefine.SKILL_INPUT).Value;
            DRSkill skillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(inputSkill.SkillID);
            if (!skillCfg.IsBeHitBreakable)
            {
                return;
            }

        }

        EntityStatus.EventFuncChangeState(OwnerFsm, BeHitStatusCore.Name);
    }
}