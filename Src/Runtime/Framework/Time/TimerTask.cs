using System;
using GameFramework;
using UnityEngine;

/// <summary>
/// 定时器任务
/// </summary>
public class TimerTask : IReference
{
    public long UID;
    /// <summary>
    /// 持续时间 毫秒
    /// </summary>
    public float Duration;
    public Action FinishCB;
    public int Times;
    private int _curTimes;

    /// <summary>
    /// 过期时间 真实时间
    /// </summary>
    public float ExpireTime;

    public void Init(long uid, float duration, Action finishCB, int times)
    {
        UID = uid;
        Duration = duration;
        FinishCB = finishCB;
        Times = times;
        _curTimes = 0;

        StartTimer();
    }

    public void StartTimer()
    {
        _curTimes++;
        ExpireTime = Time.realtimeSinceStartup + (Duration * TimeUtil.MS2S);
    }

    /// <summary>
    /// 是否已经完成
    /// </summary>
    /// <returns></returns>
    public bool IsCompleted()
    {
        return Times > 0 && _curTimes >= Times;
    }

    public void Clear()
    {
        UID = -1;
        Duration = 0;
        FinishCB = null;
        Times = 1;
        _curTimes = 0;
    }

    public static TimerTask Create()
    {
        return ReferencePool.Acquire<TimerTask>();
    }

    public static void Release(TimerTask task)
    {
        ReferencePool.Release(task);
    }
}