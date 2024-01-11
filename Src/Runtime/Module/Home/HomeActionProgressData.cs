using System;
using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 家园动作进度数据 挂在在每个土地或者采集物上
/// </summary>
public class HomeActionProgressData : MonoBehaviour
{
    private const string BACK_PROTECT_TIMER_KEY = "BACK_PROTECT_TIMER_KEY";
    /// <summary>
    /// 进度充满的时间戳 只在设置HoldToFull才有效
    /// </summary>
    public long FullTimeStamp { get; private set; }
    private Action<HomeActionProgressData> _onProgressHoldFull;//进度满了后回调
    private Tweener _tween;
    /// <summary>
    /// 正在holdToFull中
    /// </summary>
    public bool IsHoldToFulling => _tween != null;

    /// <summary>
    /// 当前是否在进度型操作中 单一动作 不会是复合动作 None表示不在
    /// </summary>
    /// <value></value>
    public eAction CurProgressAction { get; private set; }

    /// <summary>
    /// 当前进度动作的进度值
    /// </summary>
    /// <value></value>
    public float CurProgressActionValue { get; private set; }
    /// <summary>
    /// 当前进度归属玩家id 0代表没有归属
    /// </summary>
    /// <value></value>
    public long CurProgressOwnerId { get; private set; }

    /// <summary>
    /// 当前进度动作的最大值
    /// </summary>
    /// <value></value>
    public float CurProgressActionMaxValue { get; private set; }

    private bool _backProtectTimerRunning;//是否正在进度返回保护计时

    private void Awake()
    {
        StopProgressLost();

        if (!InitHomeProgressLostSpeed)
        {
            InitHomeProgressLostSpeed = true;
            int lostTime = TableUtil.GetGameValue(eGameValueID.homeActionLostSpeed).Value;//配置几秒流完
            if (lostTime <= 0)
            {
                Log.Error($"homeActionLostSpeed配置错误:{lostTime}");
                lostTime = 1;
            }
            HomeProgressLostSpeed = 1 / (float)lostTime;
        }
    }

    private void Update()
    {
        if (!_backProtectTimerRunning && !IsHoldToFulling)
        {
            UpdateProgressActionValueLost();
        }
    }

    private void OnDestroy()
    {
        StopBackProtectTimer();
    }

    //在进度型操作中 进度值会一直衰减
    private void UpdateProgressActionValueLost()
    {
        if (CurProgressAction == eAction.None)
        {
            return;
        }

        if (CurProgressActionValue >= CurProgressActionMaxValue)//如果已经满了 不会衰减 此时在等服务器回包进入下一个阶段
        {
            StopProgressLost();
            return;
        }

        CurProgressActionValue -= HomeProgressLostSpeed * CurProgressActionMaxValue * Time.deltaTime;
        CurProgressActionValue = Mathf.Max(0, CurProgressActionValue);

        if (CurProgressActionValue.ApproximatelyEquals(0))
        {
            SetCurProgressOwnerId(0);

            StopProgressLost();
        }
    }

    private void StartProgressLost()
    {
        enabled = true;
    }

    private void StopProgressLost()
    {
        enabled = false;
    }

    public void SetCurProgressOwnerId(long id)
    {
        CurProgressOwnerId = id;
    }

    /// <summary>
    /// 检查某个玩家是否能够操作进度
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public virtual bool CheckCanOperate(long playerId)
    {
        if (HomeModuleCore.IsInited)//家园里的不需要归属检查 都可以叠加操作
        {
            return true;
        }

        if (CurProgressActionValue <= 0)
        {
            if (CurProgressOwnerId != 0)
            {
                Log.Error($"home action progress is empty ,but owner is not empty:{CurProgressOwnerId}");
                CurProgressOwnerId = 0;
            }
            return true;
        }

        if (CurProgressOwnerId == 0)//没有归属
        {
            return true;
        }

        return CurProgressOwnerId == playerId;
    }

    /// <summary>
    /// 开始一个有进度的动作 之后就会有了进度值逻辑
    /// </summary>
    /// <param name="action"></param>
    /// <param name="maxValue"></param>
    public void StartProgressAction(eAction action, float maxValue)
    {
        //拾取动作比较特殊 不走进度但是要走进度icon  https://linear.app/project-linco/issue/LNCO-4361/砍树挖矿采草时，需要显示名字和提示等信息
        if ((action & eAction.Pick) == 0 && (PROGRESS_ACTION_MASK & action) == 0)
        {
            Log.Error($"不支持的进度操作:{action}");
            return;
        }

        SetCurProgressOwnerId(0);

        StopHoldToFull(true);

        StopBackProtectTimer();

        CurProgressAction = action;
        CurProgressActionMaxValue = maxValue;
        CurProgressActionValue = 0;
    }

    /// <summary>
    /// 结束进度值逻辑
    /// </summary>
    public void EndProgressAction()
    {
        CurProgressAction = eAction.None;
        CurProgressActionMaxValue = 0;
        CurProgressActionValue = 0;

        SetCurProgressOwnerId(0);

        StopHoldToFull(true);

        StopBackProtectTimer();
    }

    /// <summary>
    /// 设置hold到满的时间
    /// </summary>
    /// <param name="fullTime">多久后充满 秒</param>
    /// <param name="onProgressHoldFull"></param>
    public void SetHoldToFull(float fullTime, Action<HomeActionProgressData> onProgressHoldFull = null)
    {
        StopHoldToFull(true);

        OnTriggerActionChangeProgress(true);

        if (fullTime <= 0)
        {
            Log.Error("fullTime必须大于0");
            onProgressHoldFull?.Invoke(this);
            return;
        }

        FullTimeStamp = TimeUtil.GetServerTimeStamp() + (long)(fullTime * TimeUtil.S2MS);
        _onProgressHoldFull = onProgressHoldFull;
        _tween = DOTween.To(() => CurProgressActionValue, x =>
        {
            CurProgressActionValue = x;
            OnHoldUpdateChangeProgress();
        }, CurProgressActionMaxValue, fullTime).SetEase(Ease.Linear);
        _tween.onComplete += () =>
        {
            CurProgressActionValue = CurProgressActionMaxValue;//防止精度问题
            _onProgressHoldFull?.Invoke(this);
            _onProgressHoldFull = null;
            OnHoldUpdateFinish();
            FullTimeStamp = 0;
            _tween = null;
        };

        StartProgressLost();
    }

    /// <summary>
    /// 停止hold到满的缓动
    /// </summary>
    public void StopHoldToFull(bool force)
    {
        if (!force)
        {
            OnTriggerActionHoldStop();
        }

        if (_tween == null)
        {
            return;
        }

        _tween.Kill();
        _tween = null;
        _onProgressHoldFull = null;
        FullTimeStamp = 0;
    }

    /// <summary>
    /// 在触发动作改变进度时调用 不会再初始化时调用
    /// </summary>
    /// <param name="isHoldAction"></param>
    protected virtual void OnTriggerActionChangeProgress(bool isHoldAction)
    {
        StartBackProtectTimer();
    }

    /// <summary>
    /// 在触发动作停止时调用 不会再初始化时调用
    /// </summary>
    protected virtual void OnTriggerActionHoldStop()
    {
        StartBackProtectTimer();
    }

    /// <summary>
    /// 在holdToFull中每次进度值改变时调用
    /// </summary>
    protected virtual void OnHoldUpdateChangeProgress()
    {
    }

    /// <summary>
    /// 在holdToFull中满了自动结束时调用
    /// </summary>
    protected virtual void OnHoldUpdateFinish()
    {
    }

    /// <summary>
    /// 直接设置进度值 会停掉hold到满的缓动 里面会保护进度范围
    /// </summary>
    /// <param name="progressValue">进度值</param>
    public void SetProgressValue(int progressValue, bool force)
    {
        if (!force)
        {
            OnTriggerActionChangeProgress(false);
        }

        StopHoldToFull(true);

        CurProgressActionValue = progressValue;

        CurProgressActionValue = Mathf.Clamp(CurProgressActionValue, 0, CurProgressActionMaxValue);

        if (CurProgressActionValue > 0)
        {
            StartProgressLost();
        }
    }

    private void StartBackProtectTimer()
    {
        if (_backProtectTimerRunning)
        {
            StopBackProtectTimer();
        }

        _backProtectTimerRunning = true;
        TimerMgr.AddTimer(UIDUtil.ToLongUID(this, BACK_PROTECT_TIMER_KEY), HOME_PROGRESS_ACTION_BACK_PROTECT_TIME, () => _backProtectTimerRunning = false);
    }

    private void StopBackProtectTimer()
    {
        if (!_backProtectTimerRunning)
        {
            return;
        }

        _ = TimerMgr.RemoveTimer(UIDUtil.ToLongUID(this, BACK_PROTECT_TIMER_KEY));
        _backProtectTimerRunning = false;
    }

    /// <summary>
    /// 检查是否进度是空信息
    /// </summary>
    /// <returns></returns>
    public bool CheckProcessIsEmptyInfo()
    {
        if (CurProgressAction == eAction.None)
        {
            return true;
        }

        if (IsHoldToFulling)
        {
            return false;
        }

        if (CurProgressActionValue > 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取初始化网络传递的进度信息 如果没有任何进度返回null
    /// </summary>
    /// <returns></returns>
    public GameMessageCore.CollectResourceProgressResult GetInitProxyProgressInfo()
    {
        if (CheckProcessIsEmptyInfo())
        {
            return null;
        }

        if (IsHoldToFulling)
        {
            return new()
            {
                TotalProgress = Mathf.CeilToInt(CurProgressActionValue),
                ProgressFullStamp = FullTimeStamp,
                ProgressOwnerId = CurProgressOwnerId,
            };
        }

        if (CurProgressActionValue <= 0)
        {
            return null;
        }

        return new()
        {
            TotalProgress = Mathf.CeilToInt(CurProgressActionValue),
            ProgressFullStamp = 0,
            ProgressOwnerId = CurProgressOwnerId,
        };
    }
}