using GameFramework.Fsm;

/// <summary>
/// 畜牧动物死亡状态
/// </summary>
public class AnimalDeathStatusCore : DeathStatusCore
{
    protected HomeAnimalCore HomeAnimal;

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        HomeAnimal = StatusCtrl.RefEntity.GetComponent<HomeAnimalCore>();

        if (StatusCtrl.RefEntity.TryGetComponent(out FindPathMove move))
        {
            move.StopMove();
        }
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        HomeAnimal = null;

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnDeathEnd()
    {
        //重写父 不要切换到幽灵状态 需要停留在死亡状态 很多操作UI状态都在动物死亡状态里 不能离开状态
    }
}