using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 定时器管理 TODO:目前功能不完善 只给一个定时器 用来统一接口和便捷使用
/// </summary>
public class TimerMgr
{
    //内部也不要直接用这个 也用单例访问Instance
    private static TimerMgr s_instance;
    public static TimerMgr Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new TimerMgr();
            }
            return s_instance;
        }
    }

    private readonly Dictionary<long, TimerTask> _timerMap = new();
    private readonly List<TimerTask> _timerList = new();

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="uid">用于全局唯一id 往往用对象hashcode即可</param>
    /// <param name="duration">持续时间 ms</param>
    /// <param name="finishCB">回调函数</param>
    /// <param name="times">次数 0代表一直循环 默认一次</param>
    public static void AddTimer(long uid, float duration, Action finishCB, int times = 1)
    {
        if (Instance._timerMap.ContainsKey(uid))
        {
            Log.Error($"TimerMgr AddTimer uid:{uid} already exist");
            return;
        }

        TimerTask task = TimerTask.Create();
        task.Init(uid, duration, finishCB, times);
        Instance._timerMap.Add(uid, task);
        Instance._timerList.Add(task);
    }

    /// <summary>
    /// 移除一个定时器 不需要检查是不是还在 内部会检查
    /// </summary>
    /// <param name="uid">用于全局唯一id 往往用对象hashcode即可</param>
    /// <returns></returns>
    public static bool RemoveTimer(long uid)
    {
        if (!Instance._timerMap.ContainsKey(uid))
        {
            return false;
        }

        TimerTask task = Instance._timerMap[uid];
        _ = Instance._timerMap.Remove(uid);
        _ = Instance._timerList.Remove(task);
        TimerTask.Release(task);
        return true;
    }

    /// <summary>
    /// 查询是否有定时器 一般不用 约定移除前不用判定 内部会判定
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static bool HasTimer(long uid)
    {
        return Instance._timerMap.ContainsKey(uid);
    }

    public void Update(float deltaTime, float unscaledDeltaTime)
    {
        float curTime = Time.realtimeSinceStartup;
        int length = _timerList.Count;
        for (int i = length - 1; i >= 0; i--)
        {
            // 存在回调中移除其他多个定时器情况
            if (i >= _timerList.Count)
            {
                continue;
            }

            TimerTask task = _timerList[i];
            if (curTime >= task.ExpireTime)
            {
                Action cb = task.FinishCB;
                if (task.IsCompleted())
                {
                    _ = Instance._timerMap.Remove(task.UID);
                    Instance._timerList.RemoveAt(i);
                    TimerTask.Release(task);
                }
                else
                {
                    task.StartTimer();
                }

                try
                {
                    cb?.Invoke();
                }
                catch (Exception e)
                {
                    Log.Error($"Timer finish cb execute Exception e:{e}");
                }
            }
        }
    }
}