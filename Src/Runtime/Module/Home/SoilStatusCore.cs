using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using static HomeDefine;
/// <summary>
/// 土地上的各种状态的基类 实体状态类直接可以在内部任意时间切换状态 不要求再update 切换时取OwnerFsm
/// </summary>
public abstract class SoilStatusCore : ComponentStatusCore<SoilStatusCtrl>
{
    private static TimeUtil.DelegateGetTimeStamp s_getNowTimeStamp = TimeUtil.GetServerTimeStamp;
    /// <summary>
    /// 客户端需要设置这个时间戳方法为获取服务器时间 TODO:
    /// </summary>
    /// <param name="getNowTimeStampFunc"></param>
    public static void SetGetNowTimestampFunc(TimeUtil.DelegateGetTimeStamp getNowTimeStampFunc)
    {
        s_getNowTimeStamp = getNowTimeStampFunc;
    }

    /// <summary>
    /// 获取当前时间戳 家园状态全部需要使用这个 因为这个会使客户端和服务器时间保持一致 客户端会使用服务器的 服务器会使用自己的
    /// </summary>
    /// <returns></returns>
    public static long GetNowTimestamp()
    {
        return s_getNowTimeStamp();
    }

    protected SoilData SoilData { get; private set; }
    /// <summary>
    /// 当前状态枚举标记
    /// </summary>
    /// <value></value>
    public abstract eSoilStatus StatusFlag { get; }

    /// <summary>
    /// 当前状态支持的家园动作 可以是多个动作二进制复合动作
    /// </summary>
    /// <value></value>
    public abstract eAction SupportAction { get; }
    /// <summary>
    /// 设置支持动作时必选 子类实现具体的动作逻辑 如果不支持该动作则不会触发该事件 内部异常时需要抛出错误
    /// </summary>
    /// <param name="action">当前执行的动作</param>
    /// <param name="actionData">动作数据 比如播种的种子cid</param>
    protected virtual void OnExecuteHomeAction(eAction action, object actionData) { }

    /// <summary>
    /// 自动进入下一个状态的时间 等于0不会自动进入 秒
    /// </summary>
    /// <value></value>
    protected abstract float AutoEnterNextStatusTime { get; }
    /// <summary>
    /// 设置自动进入时必选 自动进入下一个状态时的具体逻辑
    /// </summary>
    protected virtual void OnAutoEnterNextStatus() { }

    public sealed override string StatusName => StatusFlag.ToString();

    private bool _isTimerNextStatus;//有开始定时去下个状态

    private bool _needInitJumpNextStatus;//需要初始化跳下个状态

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        SoilData = StatusCtrl.GetComponent<SoilData>();
        SoilData.SaveData.SoilStatus = StatusFlag;

        if (SupportAction != eAction.None)
        {
            StatusCtrl.SoilEvent.MsgExecuteAction += OnMsgExecuteAction;
        }

        _needInitJumpNextStatus = false;
        //如果是初始化状态就保留初始化数据中的进入时间戳 否则都要重新设置
        if (fsm.HasData(SoilStatusDataName.IS_INIT_STATUS))
        {
            OnEnterInitStatus(fsm);
        }
        else//正常进入
        {
            ResetEnterStatusStamp();

            if (AutoEnterNextStatusTime > 0)
            {
                _isTimerNextStatus = true;
                TimerMgr.AddTimer(GetHashCode(), AutoEnterNextStatusTime * TimeUtil.S2MS, OnAutoEnterNextStatus);
            }
        }
    }

    /// <summary>
    /// 在进入状态时为初始化状态时的处理 主要是在服务器启动时会根据DB回复之前的状态 需要重新计算下一个状态
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void OnEnterInitStatus(IFsm<SoilStatusCtrl> fsm)
    {
        if (AutoEnterNextStatusTime > 0)
        {
            float costTime = (GetNowTimestamp() - SoilData.SaveData.StatusStartStamp) * TimeUtil.MS2S;
            float initRemainTime = AutoEnterNextStatusTime - costTime;
            if (initRemainTime < 0)//已经到了下一个状态的时间  直接跳到下一个状态
            {
                SoilData.SaveData.StatusStartStamp += (long)(AutoEnterNextStatusTime * TimeUtil.S2MS);//下个状态的开始时间会延后本次自动切换的时间
                _needInitJumpNextStatus = true;
                //不去掉出初始化标记 让下个状态继续走初始化状态
            }
            else//还没到下一个状态的时间 继续等待 并删掉初始化标记
            {
                _ = fsm.RemoveData(SoilStatusDataName.IS_INIT_STATUS);

                _isTimerNextStatus = true;
                TimerMgr.AddTimer(GetHashCode(), initRemainTime * TimeUtil.S2MS, OnAutoEnterNextStatus);
            }
        }
        else
        {
            _ = fsm.RemoveData(SoilStatusDataName.IS_INIT_STATUS);
        }
    }

    protected override void OnLeave(IFsm<SoilStatusCtrl> fsm, bool isShutdown)
    {
        if (SupportAction != eAction.None)
        {
            StatusCtrl.SoilEvent.MsgExecuteAction -= OnMsgExecuteAction;
        }

        if (_isTimerNextStatus)
        {
            _ = TimerMgr.RemoveTimer(GetHashCode());
            _isTimerNextStatus = false;
        }

        SoilData = null;

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnUpdate(IFsm<SoilStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (_needInitJumpNextStatus)//为什么放到这里 不在enter里面直接跳转  是因为防止子类override enter的时候 先base.enter直接跳转释放了 后面的逻辑会出错
        {
            OnAutoEnterNextStatus();
        }
    }

    private void OnMsgExecuteAction(eAction action, object actionData)
    {
        if (!CheckSupportAction(action))
        {
            throw new System.Exception("当前状态不支持该动作，不应该触发该事件");
        }

        OnExecuteHomeAction(action, actionData);
    }

    /// <summary>
    /// 检查是否支持某个家园动作
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool CheckSupportAction(eAction action)
    {
        return (SupportAction & action) != 0;
    }

    /// <summary>
    /// 统一使用突然状态枚举来切换状态 一一对应
    /// </summary>
    /// <param name="status"></param>
    protected void ChangeState(eSoilStatus status)
    {
        ChangeState(OwnerFsm, status.ToString());
    }

    /// <summary>
    /// 重置进入状态时间戳
    /// </summary>
    private void ResetEnterStatusStamp()
    {
        SoilData.SaveData.StatusStartStamp = GetNowTimestamp();
    }
}