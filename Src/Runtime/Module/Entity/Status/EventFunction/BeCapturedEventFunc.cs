using UnityGameFramework.Runtime;
/// <summary>
/// 被捕获事件
/// </summary>
public class BeCapturedEventFunc : EntityStatusEventFunctionBase
{
    public override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeCaptured += OnEntityBeCapture;
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeCaptured -= OnEntityBeCapture;
    }

    private void OnEntityBeCapture(long fromId)
    {
        OwnerFsm.SetData<VarInt64>(StatusDataDefine.CAPTURE_FROM_ID, fromId);
        EntityStatus.EventFuncChangeState(OwnerFsm, BeCapturedStatusCore.Name);
    }
}