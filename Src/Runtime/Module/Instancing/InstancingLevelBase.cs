/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本关卡
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/InstancingLevelBase.cs
 * 
 */
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class InstancingLevelBase : MonoBehaviour, IInstancingLevel
{
    public int LevelIndex;                                                          //关卡索引
    public InstancingLevelData LevelData;                                           //关卡数据
    public eInstancingStatusType StatusType = eInstancingStatusType.InstancingInactive;       //关卡状态
    /// <summary>
    /// 初始化关卡数据
    /// </summary>
    public virtual void InitData(int index, InstancingLevelData levelData, eInstancingStatusType statusType)
    {
        LevelIndex = index;
        LevelData = levelData;
        StatusType = statusType;
    }
    /// <summary>
    /// 运行关卡
    /// </summary>
    public virtual bool RunLevel()
    {
        if (StatusType != eInstancingStatusType.InstancingInactive)
        {
            Log.Error($"InstancingLevel StartLevel Error: StatusType = {StatusType}");
            return false;
        }
        StatusType = eInstancingStatusType.InstancingRunning;
        return true;
    }

    /// <summary>
    /// 完成关卡
    /// </summary>
    /// <param name="isSuccess"></param>
    public virtual bool CompleteLevel(bool isSuccess)
    {
        if (StatusType != eInstancingStatusType.InstancingRunning)
        {
            Log.Error($"InstancingLevel CompleteLevel Error: StatusType = {StatusType} isSuccess = {isSuccess}");
            return false;
        }
        StatusType = isSuccess ? eInstancingStatusType.InstancingSuccess : eInstancingStatusType.InstancingFailure;
        return true;
    }

    /// <summary>
    /// 重置关卡
    /// </summary>
    public virtual bool ResetLevel()
    {
        StatusType = eInstancingStatusType.InstancingInactive;
        return true;
    }

    /// <summary>
    /// 同步关卡更新
    /// </summary>
    /// <param name="levelData"></param>
    public virtual bool SyncLevelUpdate(GameMessageCore.InstancingLevelData levelData)
    {
        StatusType = levelData.Status;
        return true;
    }
}
