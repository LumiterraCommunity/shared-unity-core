/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本关卡管理接口
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/IInstancingMgr.cs
 * 
 */

using System;
using GameMessageCore;
/// <summary>
/// 副本关卡接口
/// </summary>
public interface IInstancingMgr
{
    /// <summary>
    /// 是否初始化
    /// </summary>
    /// <param name="index"></param>
    bool IsInit { get; set; }
    /// <summary>
    /// 当前关卡时间
    /// <summary>
    long CurLevelStartTime { get; set; }
    /// <summary>
    /// 当前关卡索引
    /// </summary>
    int CurLevelIndex { get; set; }
    /// 设置当前关卡
    /// </summary>
    /// <param name="index"></param>
    bool SetCurLevel(int index);
    /// <summary>
    /// 完成关卡
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSuccess"></param>
    /// <param name="isReward"></param>
    bool CompleteLevel(int index, bool isSuccess, bool isReward = true);

    /// <summary>
    /// 重置关卡
    /// </summary>
    /// <param name="index"></param>
    bool ResetLevel(int index);

    eInstancingStatusType GetLevelStatus(int levelIndex);

}

