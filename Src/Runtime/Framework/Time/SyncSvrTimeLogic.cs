using System;

using UnityGameFramework.Runtime;
/// <summary>
/// 和相对自己的服务器进行时间同步逻辑
/// </summary>
public class SyncSvrTimeLogic
{
    private const int SYNC_SVR_TIME_NORMAL_TIMER = 60 * 1000;//正常速度同步时间 间隔时间 ms
    private const int SYNC_SVR_TIME_FAST_TIMER = 10 * 1000;//快速同步时间 间隔时间 ms
    private const string TIMER_KEY_SYNC_SVR_TIME_NORMAL = "TIMER_KEY_SYNC_SVR_TIME_NORMAL";//正常速度同步时间
    private const string TIMER_KEY_SYNC_SVR_TIME_FAST = "TIMER_KEY_SYNC_SVR_TIME_FAST";//快速同步时间
    private const long JUDGE_FAST_DELAY = 200;//判断需要快速同步的延迟时间 ms
    private const long JUDGE_NORMAL_DELAY = 70;//判断需要正常同步的延迟时间 ms

    private long _lastSyncSvrStamp = -1;//最近同步的服务器时间戳
    private DateTime _lastSyncSvrStampClientTime;//最近同步服务器时间时 对应客户端运行时间 不一定要UTC 只是为了算差值
    private long _buffLastSyncTimeSpaceTime = -1;//最近一次来回时间差 ms

    private readonly Action _reqQuerySvrTimeFunc;//请求查询服务器时间方法
    private bool _isHaveFastTimer;//是否有快速同步时间定时请求

    public bool TimerReqEnable { get; private set; }//是否开着定时请求的

    /// <summary>
    /// 获取服务器时间戳
    /// </summary>
    /// <value></value>
    public long ServerLocalTimestamp
    {
        get
        {
            if (_lastSyncSvrStamp < 0)
            {
                Log.Error($"未同步过服务器时间");
                return TimeUtil.GetTimeStamp();
            }
            TimeSpan t = DateTime.Now - _lastSyncSvrStampClientTime;
            return _lastSyncSvrStamp + (long)t.TotalMilliseconds;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reqQuerySvrTimeFunc">请求查询服务器时间的方法 可以为null 为null时需要自己情趣服务器时间</param>
    public SyncSvrTimeLogic(Action reqQuerySvrTimeFunc)
    {
        _reqQuerySvrTimeFunc = reqQuerySvrTimeFunc;

        StartSyncSvrTime();
    }

    public void Dispose()
    {
        if (TimerReqEnable)
        {
            StopSyncSvrTime();
        }
    }

    /// <summary>
    /// 开始启动定时请求同步服务 不开始只要有地方调用同步时间函数接口也能用 只是不会自动同步
    /// </summary>
    public void StartSyncSvrTime()
    {
        if (TimerReqEnable)
        {
            StopSyncSvrTime();
        }

        if (_reqQuerySvrTimeFunc == null)
        {
            return;
        }

        //以前同步过 而且已经很精准
        if (_buffLastSyncTimeSpaceTime is > 0 and < JUDGE_NORMAL_DELAY)
        {
            return;
        }

        TimerReqEnable = true;

        Log.Info($"startSyncSvrTime");

        TimerMgr.AddTimer(UIDUtil.ToLongUID(this, TIMER_KEY_SYNC_SVR_TIME_NORMAL), SYNC_SVR_TIME_NORMAL_TIMER, ReqQuerySvrTime, 0);

        //以前没同步过 或者同步过但是延迟很大 仍然需要快速同步
        if (_buffLastSyncTimeSpaceTime is < 0 or > JUDGE_FAST_DELAY)
        {
            _isHaveFastTimer = true;
            TimerMgr.AddTimer(UIDUtil.ToLongUID(this, TIMER_KEY_SYNC_SVR_TIME_FAST), SYNC_SVR_TIME_FAST_TIMER, ReqQuerySvrTime, 0);
        }

        ReqQuerySvrTime();
    }

    /// <summary>
    /// 停止同步服务 只会停止各种定时器等 之前请求的数据依旧保留
    /// </summary>
    public void StopSyncSvrTime()
    {
        if (!TimerReqEnable)
        {
            return;
        }
        TimerReqEnable = false;

        Log.Info($"stopSyncSvrTime last spaceTime ={_buffLastSyncTimeSpaceTime}");

        _ = TimerMgr.RemoveTimer(UIDUtil.ToLongUID(this, TIMER_KEY_SYNC_SVR_TIME_NORMAL));

        if (_isHaveFastTimer)
        {
            _ = TimerMgr.RemoveTimer(UIDUtil.ToLongUID(this, TIMER_KEY_SYNC_SVR_TIME_FAST));
            _isHaveFastTimer = false;
        }
    }

    private void ReqQuerySvrTime()
    {
        _reqQuerySvrTimeFunc?.Invoke();
    }

    /// <summary>
    /// 同步一次服务器时间
    /// </summary>
    /// <param name="svrStamp">本次服务器时间戳</param>
    /// <param name="reqClientStamp">本次当时请求的客户端时间戳</param>
    public void SyncSvrTime(long svrStamp, long reqClientStamp)
    {
        long spaceTime = TimeUtil.GetTimeStamp() - reqClientStamp;//网络来回时间
        // Log.Info($"syncSvrTime svrStamp =${svrStamp} spaceTime ={spaceTime} reqClientStamp ={reqClientStamp}");

        //需要收敛 非收敛的直接跳过
        if (_buffLastSyncTimeSpaceTime > 0 && spaceTime >= _buffLastSyncTimeSpaceTime)
        {
            return;
        }

        _buffLastSyncTimeSpaceTime = spaceTime;

        //同步时间
        _lastSyncSvrStamp = svrStamp + (spaceTime / 2);//加上一半的网络来回时间差
        _lastSyncSvrStampClientTime = DateTime.Now;

        //已经很精确 不需要继续同步
        if (spaceTime < JUDGE_NORMAL_DELAY)
        {
            // StopSyncSvrTime();
            return;
        }

        //比较精确 不需要快速同步 如果已经停止快速就不理会
        if (spaceTime < JUDGE_FAST_DELAY && _isHaveFastTimer)
        {
            _ = TimerMgr.RemoveTimer(UIDUtil.ToLongUID(this, TIMER_KEY_SYNC_SVR_TIME_FAST));
            _isHaveFastTimer = false;
        }
    }
}