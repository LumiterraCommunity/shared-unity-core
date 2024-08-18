using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 单块土地上的状态控制器
/// </summary>
public class SoilStatusCtrl : MonoBehaviour
{
    /// <summary>
    /// 获取引用的土地core
    /// </summary>
    public HomeSoilCore HomeSoilCore { get; private set; }

    /// <summary>
    /// 获取土地事件组件 专门给状态用的 外部不要使用
    /// </summary>
    public SoilEvent SoilEvent { get; private set; }

    /// <summary>
    /// 当前状态机 状态机名字就是EntityBase Root GetInstanceID()
    /// </summary>
    /// <value></value>
    public IFsm<SoilStatusCtrl> Fsm { get; private set; }

    private static IFsmManager s_cacheFsmMgr;//不用每次去GameFrameworkEntry获取 获取代码底层是遍历 有性能损耗 后续优化成字典后就可以不用缓存了

    private void OnDestroy()
    {
        ClearFsm();
    }

    private void ClearFsm()
    {
        if (Fsm == null)
        {
            return;
        }

        _ = GetFsmManager().DestroyFsm(Fsm);
        Fsm = null;
    }


    internal void Init(HomeSoilCore homeSoilCore, SoilEvent soilEvent)
    {
        HomeSoilCore = homeSoilCore;
        SoilEvent = soilEvent;
    }

    /// <summary>
    /// 初始化状态机 需要给定固定的状态实例
    /// </summary>
    /// <param name="states"></param>
    public void InitFsm(params FsmState<SoilStatusCtrl>[] states)
    {
        if (Fsm != null)
        {
            ClearFsm();
        }

        Fsm = GetFsmManager().CreateFsm(GetHashCode().ToString(), this, states);
    }

    /// <summary>
    /// 启动状态 需要给定启动状态
    /// </summary>
    /// <typeparam name="TStartStatus"></typeparam>
    public void StartStatus<TStartStatus>() where TStartStatus : FsmState<SoilStatusCtrl>
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
    public void StartStatus(HomeDefine.eSoilStatus status)
    {
        if (Fsm == null)
        {
            Log.Error($"start status when not init fsm name={gameObject.name}");
            return;
        }

        Fsm.Start(status.ToString());
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
    /// 统一创建土地状态的方法 后面方便统一走对象池什么的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateStatus<T>() where T : SoilStatusCore, new()
    {
        return new T();
    }

    /// <summary>
    /// 统一销毁土地状态的方法 后面方便统一走对象池什么的
    /// </summary>
    /// <param name="status"></param>
    public static void DestroyStatus(SoilStatusCore status)
    {

    }
}