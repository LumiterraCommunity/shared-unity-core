#define SHARED_CORE_TIME_UTIL
using System;
using UnityGameFramework.Runtime;

public static class TimeUtil
{
    /// <summary>
    /// 获取时间戳代理定义 用在需要选择获取时间戳方法的地方
    /// </summary>
    /// <returns></returns>
    public delegate long DelegateGetTimeStamp();
    /// <summary>
    /// 秒转毫秒
    /// </summary>
    public static readonly float S2MS = 1000;
    /// <summary>
    /// 毫秒转秒
    /// </summary>
    public static readonly float MS2S = 1 / S2MS;

    public static readonly DateTime DateForm = new(1970, 1, 1, 0, 0, 0, 0);
    public static readonly int SecondsOfMinute = 60;
    public static readonly int SecondsOfHour = 3600;
    public static readonly int SecondsOfDay = 86400;

    private static SyncSvrTimeLogic s_syncSvrTimeLogic;

    /// <summary>
    /// 是否初始化过
    /// </summary>
    public static bool IsInitedSyncTimeLogic => s_syncSvrTimeLogic != null;

    /// <summary>
    /// 初始化时间同步逻辑
    /// </summary>
    /// <param name="reqQuerySvrTimeFunc">请求查询服务器时间的方法 可以为null 为null时需要自己情趣服务器时间</param>
    public static void InitSyncSvrTimeLogic(Action reqQuerySvrTimeFunc)
    {
        UnInitSyncSvrTimeLogic();

        s_syncSvrTimeLogic = new SyncSvrTimeLogic(reqQuerySvrTimeFunc);
    }

    /// <summary>
    /// 释放时间同步逻辑
    /// </summary>
    public static void UnInitSyncSvrTimeLogic()
    {
        if (s_syncSvrTimeLogic != null)
        {
            s_syncSvrTimeLogic.Dispose();
            s_syncSvrTimeLogic = null;
        }
    }

    /// <summary>
    /// 开始同步服务器时间 初始化后会自动调用 不用额外调用
    /// </summary>
    public static void StartSyncSvrTime()
    {
        if (s_syncSvrTimeLogic == null)
        {
            Log.Error("TimeUtil StartSyncSvrTime syncSvrTimeLogic not init");
            return;
        }

        if (s_syncSvrTimeLogic.TimerReqEnable)
        {
            return;
        }

        s_syncSvrTimeLogic.StartSyncSvrTime();
    }

    /// <summary>
    /// 停止同步服务器时间
    /// </summary>
    public static void StopSyncSvrTime()
    {
        if (s_syncSvrTimeLogic == null)
        {
            Log.Error("TimeUtil StopSyncSvrTime syncSvrTimeLogic not init");
            return;
        }

        s_syncSvrTimeLogic.StopSyncSvrTime();
    }

    /// <summary>
    /// 同步一次服务器时间
    /// </summary>
    /// <param name="svrStamp">本次服务器时间戳</param>
    /// <param name="reqClientStamp">本次当时请求的客户端时间戳</param>
    public static void SyncSvrTime(long svrStamp, long reqClientStamp)
    {
        if (s_syncSvrTimeLogic == null)
        {
            Log.Error("TimeUtil syncSvrTimeLogic not init");
            return;
        }

        s_syncSvrTimeLogic.SyncSvrTime(svrStamp, reqClientStamp);
    }

    /// <summary>
    /// 获取当前本机时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        long currentTicks = DateTime.UtcNow.Ticks;
        long curMs = (currentTicks - DateForm.Ticks) / 10000;
        return curMs;
    }

    /// <summary>
    /// 获取当前服务器时间戳
    /// !!尽量不要在update里面使用，比如计算倒计时表现（该方法获得的时间可能会有抖动的）
    /// </summary>
    /// <returns></returns>
    public static long GetServerTimeStamp()
    {
        if (s_syncSvrTimeLogic == null)
        {
            Log.Error("未初始化时间同步逻辑");
            return GetTimeStamp();
        }

        return s_syncSvrTimeLogic.ServerLocalTimestamp;
    }

    public static long GetTimeStampByInputString(string inputStr)
    {
        string[] strList = inputStr.Split("-");
        int[] timeList = new int[6];
        for (int i = 0; i < timeList.Length; i++)
        {
            if (i < strList.Length && int.TryParse(strList[i], out int time))
            {
                timeList[i] = time;
            }
            else
            {
                timeList[i] = 0;
            }
        }
        DateTime curDateTime = new(timeList[0], timeList[1], timeList[2], timeList[3], timeList[4], timeList[5], 0, DateTimeKind.Utc);
        return DataTime2TimeStamp(curDateTime);
    }
    public static long DataTime2TimeStamp(DateTime dateTime)
    {
        TimeSpan ts = dateTime - DateForm;
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static DateTime TimeStamp2DataTime(long timestamp)
    {
        DateTime curDateTime = DateForm.AddMilliseconds(timestamp);
        return curDateTime;
    }

    // 获取当天结束的时间
    public static DateTime GetDayEndTime()
    {
        long curServerTimestamp = GetServerTimeStamp();
        DateTime curDateTime = TimeStamp2DataTime(curServerTimestamp);
        DateTime endDataTime = new(curDateTime.Year, curDateTime.Month, curDateTime.Day, 23, 59, 59);
        return endDataTime;
    }

    // 秒 转 时:分:秒 
    public static string SecondsToHMS(int nSeconds)
    {
        if (nSeconds < 0)
        {
            nSeconds = 0;
        }
        // int day = Convert.ToInt32(decimal.Floor(nSeconds / SecondsOfDay));

        int hour = Convert.ToInt32(decimal.Floor(nSeconds / SecondsOfHour));
        nSeconds %= SecondsOfHour;
        int minute = Convert.ToInt32(decimal.Floor(nSeconds / SecondsOfMinute));
        nSeconds %= SecondsOfMinute;
        return $"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}:{nSeconds.ToString().PadLeft(2, '0')}";
    }
    // 秒 转 分:秒 
    public static string SecondsToMS(int nSeconds)
    {
        if (nSeconds < 0)
        {
            nSeconds = 0;
        }
        int minute = Convert.ToInt32(decimal.Floor(nSeconds / SecondsOfMinute));
        nSeconds %= SecondsOfMinute;
        return $"{minute.ToString().PadLeft(2, '0')}:{nSeconds.ToString().PadLeft(2, '0')}";
    }
    /// <summary>
    /// 服务器会获取本地时间戳，客户端会获取服务器时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetCommonTimeStamp()
    {
        if (GFEntryCore.GFEntryType == GFEntryType.Server)
        {
            return GetTimeStamp();
        }
        else
        {
            return GetServerTimeStamp();
        }
    }

    public static string GetTimeHHMMSS(int time)
    {
        int hour = time / 3600;
        int minute = time % 3600 / 60;
        int second = time % 60;
        return $"{hour:D2}:{minute:D2}:{second:D2}";
    }
}