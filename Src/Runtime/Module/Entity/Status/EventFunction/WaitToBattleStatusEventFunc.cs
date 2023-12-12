/* 
 * @Author XQ
 * @Date 2022-08-15 19:31:15
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/EventFunction/WaitToBattleStatusEventFunc.cs
 */

using UnityGameFramework.Runtime;
/// <summary>
/// 等待进入战斗状态事件方法  和 OnInputSkillInBattleStatusEventFunc 对立
/// </summary>
public class WaitToBattleStatusEventFunc : EntityStatusEventFunctionBase
{
    public override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.InputSkillRelease += OnInputSkillRelease;
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.InputSkillRelease -= OnInputSkillRelease;
    }

    protected virtual void OnInputSkillRelease(InputSkillReleaseData inputData)
    {
        StatusCtrl.RefEntity.SetForward(inputData.Dir);

        OwnerFsm.SetData<VarInputSkill>(StatusDataDefine.SKILL_INPUT, inputData);
        EntityStatus.EventFuncChangeState(OwnerFsm, SkillAccumulateStatusCore.Name);
    }
}