using GameFramework.Fsm;
using UnityEngine;

/// <summary>
/// 挂载在unity组件上状态控制器管理的状态 可以直接可以在内部任意时间切换状态 不要求在update 切换时取OwnerFsm
/// </summary>
public class ComponentStatusCore<TStatusCtrl> : FsmState<TStatusCtrl> where TStatusCtrl : MonoBehaviour
{
    public static string Name = "ComponentStatusCore";

    public override string StatusName => Name;

    /// <summary>
    /// 状态的owner状态机 用来切换状态和查看状态
    /// </summary>
    /// <value></value>
    protected IFsm<TStatusCtrl> OwnerFsm { get; private set; }
    /// <summary>
    /// 状态控制器 挂载的TStatusCtrl组件  事件监听就是使用这个组件上的功能
    /// </summary>
    /// <value></value>
    protected TStatusCtrl StatusCtrl { get; private set; }

    protected override void OnInit(IFsm<TStatusCtrl> fsm)
    {
        base.OnInit(fsm);

        OwnerFsm = fsm;
        StatusCtrl = fsm.Owner;
    }
}