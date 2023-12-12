/*
 * @Author: xiang huan
 * @Date: 2022-08-12 13:59:29
 * @Description: 受击移动事件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/EventFunction/BeHitMoveEventFunc.cs
 * 
 */
using UnityGameFramework.Runtime;
public class BeHitMoveEventFunc : EntityStatusEventFunctionBase
{
    public override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeHitMove += OnEntityBeHitMove;
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeHitMove -= OnEntityBeHitMove;
    }

    private void OnEntityBeHitMove(int duration)
    {
        //蓄力和前摇需要判断是否能打断
        // if (EntityStatus.StatusName == SkillAccumulateStatusCore.Name || EntityStatus.StatusName == SkillForwardStatusCore.Name)
        // {
        //     int skillID = OwnerFsm.GetData<VarInt32>(StatusDataDefine.SKILL_ID).Value;
        //     DRSkill skillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        //     if (!skillCfg.AccuBreakable)
        //     {
        //         return;
        //     }
        // }
        //跳转受击移动状态
        OwnerFsm.SetData<VarInt32>(StatusDataDefine.BE_HIT_MOVE_TIME, duration);
        EntityStatus.EventFuncChangeState(OwnerFsm, BeHitMoveStatusCore.Name);
    }
}