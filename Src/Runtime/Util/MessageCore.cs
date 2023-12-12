using System;
using GameMessageCore;

/// <summary>
/// 共享库的消息中心
/// </summary>
public static class MessageCore
{
    /// <summary>
    /// 家园某个已使用的土地肥沃度发生变化 T0:土地唯一ID
    /// </summary>
    public static Action<ulong> HomeOneSoilUsedFertileChanged = delegate { };
    /// <summary>
    /// 家园某个已使用的动物幸福值发生变化 T0:动物唯一ID
    /// </summary>
    public static Action<ulong> HomeOneAnimalUsedHappyChanged = delegate { };

    /// <summary>
    /// 副本关卡初始化完成
    /// </summary>
    public static Action InstancingLevelInitFinish = delegate { };
    /// <summary>
    /// 副本关卡状态更新 p0 = 关卡索引 p1 = 关卡状态
    /// </summary>
    public static Action<int, eInstancingStatusType> LevelStatusUpdate = delegate { };
}