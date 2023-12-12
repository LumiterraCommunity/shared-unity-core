public class BeStunEventFunc : EntityStatusEventFunctionBase
{
    public override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityReceiveStunEffect += OnReceiveStunEffect;
        // entityEvent.EntityRemoveStunEffect += OnRemoveStunEffect;
    }

    public override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityReceiveStunEffect -= OnReceiveStunEffect;
        // entityEvent.EntityRemoveStunEffect -= OnRemoveStunEffect;
    }

    private void OnReceiveStunEffect()
    {
        EntityStatus.EventFuncChangeState(OwnerFsm, StunStatusCore.Name);
    }
}