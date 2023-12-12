/*
 * @Author: xiang huan
 * @Date: 2022-08-07 10:29:02
 * @Description: 死亡灵魂状态, 用于死亡后的状态
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/DeathSoulStatusCore.cs
 * 
 */
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameMessageCore;

/// <summary>
/// 死亡灵魂状态通用状态基类
/// </summary>
public class DeathSoulStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "soul";

    public override string StatusName => Name;

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);
        StatusCtrl.RefEntity.EntityEvent.EnterDeathSoul?.Invoke();

    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        StatusCtrl.RefEntity.EntityEvent.ExitDeathSoul?.Invoke();
        base.OnLeave(fsm, isShutdown);
    }

    protected override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeReborn += OnBeReborn;
    }

    protected override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeReborn -= OnBeReborn;
    }

    /// <summary>
    /// 复活
    /// </summary>
    /// <value></value>
    protected virtual void OnBeReborn()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }

    public bool CheckCanMove()
    {
        return false;
    }

    public bool CheckCanSkill(int skillId)
    {
        return false;
    }
}