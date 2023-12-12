
using GameFramework.Fsm;

/** 
* @Author XQ
* @Date 2022-08-11 20:24:17
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/EventFunction/EntityStatusEventFunctionBase.cs
*/

/// <summary>
/// 实体状态上的事件功能基类 能反查到状态组件 就也能打到gameObject
/// </summary>
public abstract class EntityStatusEventFunctionBase : IStatusEventFunction
{
    /// <summary>
    /// 挂载当前时间功能的z实体状态
    /// </summary>
    protected ListenEventStatusCore EntityStatus;
    /// <summary>
    /// 状态的owner状态机 用来切换状态和查看状态
    /// </summary>
    /// <value></value>
    protected IFsm<EntityStatusCtrl> OwnerFsm { get; private set; }
    /// <summary>
    /// 状态控制器 挂载EntityBase上的EntityStatusCtrl组件  事件监听就是使用这个组件上的功能
    /// </summary>
    /// <value></value>
    protected EntityStatusCtrl StatusCtrl { get; private set; }

    public void Init(ListenEventStatusCore status, IFsm<EntityStatusCtrl> fsm, EntityStatusCtrl statusCtrl)
    {
        EntityStatus = status;
        OwnerFsm = fsm;
        StatusCtrl = statusCtrl;
    }

    public virtual void Clear()
    {
        EntityStatus = null;
        OwnerFsm = null;
        StatusCtrl = null;
    }

    public abstract void AddEvent(EntityEvent entityEvent);

    public abstract void RemoveEvent(EntityEvent entityEvent);
}