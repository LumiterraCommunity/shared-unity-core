/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本关卡
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/InstancingLevelBase.cs
 * 
 */
using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class InstancingLevelBase : MonoBehaviour, IInstancingLevel
{
    public int LevelIndex;                                                          //关卡索引
    public InstancingLevelData LevelData;                                           //关卡数据
    public eInstancingStatusType StatusType = eInstancingStatusType.InstancingInactive;       //关卡状态
    public bool IsReward = true;                                                      //是否关卡奖励
    public int[] EventList;                                                         //关卡事件ID列表
    public int LevelScore;                                                          //关卡评分
    public int MaxLevelScore;                                                       //最大关卡评分
    public List<SceneTriggerEvent> SceneTriggerEvents = new();                      //场景触发器事件列表

    /// <summary>
    /// 初始化关卡数据
    /// </summary>
    public virtual void InitData(int index, InstancingLevelData levelData, eInstancingStatusType statusType, int[] eventList, int maxLevelScore)
    {
        LevelIndex = index;
        LevelData = levelData;
        StatusType = statusType;
        EventList = eventList;
        LevelScore = 0;
        MaxLevelScore = maxLevelScore;
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
        AddLevelEvent();
        return true;
    }

    /// <summary>
    /// 完成关卡
    /// </summary>
    /// <param name="isSuccess"></param>
    public virtual bool CompleteLevel(bool isSuccess, bool isReward = true)
    {
        if (StatusType != eInstancingStatusType.InstancingRunning)
        {
            Log.Error($"InstancingLevel CompleteLevel Error: StatusType = {StatusType} isSuccess = {isSuccess}");
            return false;
        }
        StatusType = isSuccess ? eInstancingStatusType.InstancingSuccess : eInstancingStatusType.InstancingFailure;
        IsReward = isReward;
        RemoveLevelEvent();
        return true;
    }

    /// <summary>
    /// 重置关卡
    /// </summary>
    public virtual bool ResetLevel()
    {
        StatusType = eInstancingStatusType.InstancingInactive;
        LevelScore = 0;
        return true;
    }

    /// <summary>
    /// 同步关卡更新
    /// </summary>
    /// <param name="levelData"></param>
    public virtual bool SyncLevelUpdate(GameMessageCore.InstancingLevelData levelData)
    {
        StatusType = levelData.Status;
        LevelScore = levelData.LevelScore;
        SyncAddLevelEvent(levelData);
        return true;
    }

    /// <summary>
    /// 同步添加关卡事件，客户端不会真正运行关卡，所以关卡事件在同步时直接创建和删除
    /// </summary>
    protected void SyncAddLevelEvent(GameMessageCore.InstancingLevelData levelData)
    {
        RemoveLevelEvent();
        for (int i = 0; i < levelData.SceneEventList.Count; i++)
        {
            SceneTriggerEvent sceneEvent = GFEntryCore.SceneTriggerEventMgr.SyncAddSceneTriggerEvent(levelData.SceneEventList[i]);
            SceneTriggerEvents.Add(sceneEvent);
        }
    }
    /// <summary>
    /// 添加关卡事件
    /// </summary>
    protected void AddLevelEvent()
    {
        if (EventList == null || EventList.Length == 0)
        {
            return;
        }
        RemoveLevelEvent();
        for (int i = 0; i < EventList.Length; i++)
        {
            SceneTriggerEvent sceneEvent = GFEntryCore.SceneTriggerEventMgr.AddSceneTriggerEvent(EventList[i]);
            SceneTriggerEvents.Add(sceneEvent);
        }
    }

    /// <summary>
    /// 删除关卡事件
    /// </summary> 
    protected void RemoveLevelEvent()
    {
        if (SceneTriggerEvents == null || SceneTriggerEvents.Count == 0)
        {
            return;
        }
        for (int i = 0; i < SceneTriggerEvents.Count; i++)
        {
            GFEntryCore.SceneTriggerEventMgr.RemoveSceneTriggerEvent(SceneTriggerEvents[i].Id);
        }
        SceneTriggerEvents.Clear();
    }
    /// <summary>
    /// 获取主事件列表
    /// </summary>

    public void GetMainEventList(List<SceneTriggerEvent> mainEventList)
    {
        mainEventList.Clear();
        for (int i = 0; i < SceneTriggerEvents.Count; i++)
        {
            if (SceneTriggerEvents[i].EventType == eSTEventType.Main)
            {
                mainEventList.Add(SceneTriggerEvents[i]);
            }
        }
    }
}
