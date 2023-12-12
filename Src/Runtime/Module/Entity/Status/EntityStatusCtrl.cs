using GameFramework.Fsm;
using GameFramework;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体上控制自身状态的功能组件 使用之前需要初始化状态机
/// </summary>
public class EntityStatusCtrl : EntityBaseComponent
{
#if UNITY_EDITOR
    public string FsmName;
#endif
    private EntityEvent _entityEvent;//缓存实体上的事件组件 节省性能

    /// <summary>
    /// 获取实体事件组件 专门给状态用的 外部不要使用
    /// </summary>
    /// <value></value>
    public EntityEvent EntityEvent
    {
        get
        {
            if (_entityEvent == null)
            {
                _entityEvent = gameObject.GetComponent<EntityEvent>();
            }
            return _entityEvent;
        }
    }

    /// <summary>
    /// 当前状态机 状态机名字就是EntityBase Root GetInstanceID()
    /// </summary>
    /// <value></value>
    public IFsm<EntityStatusCtrl> Fsm { get; private set; }

    private static IFsmManager s_cacheFsmMgr;//不用每次去GameFrameworkEntry获取 获取代码底层是遍历 有性能损耗 后续优化成字典后就可以不用缓存了

    private void OnDestroy()
    {
        if (Fsm != null)
        {
            _ = GetFsmManager().DestroyFsm(Fsm);
            Fsm = null;
        }
        _entityEvent = null;
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        FsmName = Fsm?.CurrentState?.StatusName ?? "error";
    }
#endif

    /// <summary>
    /// 初始化状态机 需要给定固定的状态实例
    /// </summary>
    /// <param name="states"></param>
    public void InitFsm(params FsmState<EntityStatusCtrl>[] states)
    {
        Fsm = GetFsmManager().CreateFsm(GetHashCode().ToString(), this, states);
    }

    /// <summary>
    /// 启动状态 需要给定启动状态
    /// </summary>
    /// <typeparam name="TStartStatus"></typeparam>
    public void StartStatus<TStartStatus>() where TStartStatus : FsmState<EntityStatusCtrl>
    {
        if (Fsm == null)
        {
            Log.Error($"start status when not init fsm name={gameObject.name}");
            return;
        }

        Fsm.Start<TStartStatus>();
    }

    /// <summary>
    /// 启动状态 需要给定启动状态
    /// </summary>
    /// <typeparam name="TStartStatus"></typeparam>
    public void StartStatus(string name)
    {
        if (Fsm == null)
        {
            Log.Error($"start status when not init fsm name={gameObject.name}");
            return;
        }

        Fsm.Start(name);
    }

    private IFsmManager GetFsmManager()
    {
        if (s_cacheFsmMgr == null)
        {
            s_cacheFsmMgr = GameFrameworkEntry.GetModule<IFsmManager>();
        }

        return s_cacheFsmMgr;
    }

    /// <summary>
    /// 统一创建实体状态的方法 后面方便统一走对象池什么的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateStatus<T>() where T : EntityStatusCore, new()
    {
        return new T();
    }

    /// <summary>
    /// 统一销毁实体状态的方法 后面方便统一走对象池什么的
    /// </summary>
    /// <param name="status"></param>
    public static void DestroyStatus(EntityStatusCore status)
    {

    }
}