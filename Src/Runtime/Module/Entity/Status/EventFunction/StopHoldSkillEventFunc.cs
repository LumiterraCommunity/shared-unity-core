using UnityGameFramework.Runtime;
/// <summary>
/// 尝试停止hold技能的事件功能
/// </summary>
public class StopHoldSkillEventFunc : EntityStatusEventFunctionBase
{
    private bool _isHoldSkill;

    public override void Clear()
    {
        _isHoldSkill = false;

        base.Clear();
    }

    public override void AddEvent(EntityEvent entityEvent)
    {
        VarInputSkill varInputSkill = OwnerFsm.GetData<VarInputSkill>(StatusDataDefine.SKILL_INPUT);
        if (varInputSkill == null)
        {
            return;
        }
        DRSkill dRSkill = varInputSkill.Value.DRSkill;
        if (dRSkill == null)
        {
            return;
        }

        if (dRSkill.IsHoldSkill)
        {
            entityEvent.TryStopHoldSkill += StopHoldSkill;
            _isHoldSkill = true;
        }
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        if (_isHoldSkill)
        {
            entityEvent.TryStopHoldSkill -= StopHoldSkill;
        }
    }

    private void StopHoldSkill()
    {
        EntityStatus.EventFuncChangeState(OwnerFsm, IdleStatusCore.Name);
    }
}