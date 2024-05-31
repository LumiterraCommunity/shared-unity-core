/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 场景触发器事件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/SceneTriggerEvent.cs
 * 
 */
using System;
using System.Collections.Generic;
using GameFramework;
using GameMessageCore;
using UnityGameFramework.Runtime;

public class SceneTriggerEvent : IReference
{
    public long Id; //事件ID

    public eSTEventType EventType = eSTEventType.Main; //事件类型
    public DRSceneEvent DRSceneEvent; //场景事件配置
    public eSceneEventStatusType StatusType = eSceneEventStatusType.SceneEventActivate;
    public eSTConditionCheckType CheckConditionType = eSTConditionCheckType.And;
    public List<STConditionBase> Conditions = new();
    public List<STActionBase> Actions = new();
    protected SceneEventData NetData = new();
    public virtual void Update()
    {
        UpdateStatusActivate();
        UpdateStatusCheckCondition();
        UpdateStatusExecuteAction();
    }

    /// <summary>
    /// 激活状态
    /// </summary>
    private void UpdateStatusActivate()
    {
        if (StatusType != eSceneEventStatusType.SceneEventActivate)
        {
            return;
        }
        StatusType = eSceneEventStatusType.SceneEventCheck;
        BroadcastSceneTriggerEventUpdate();
    }
    /// <summary>
    /// 检查条件状态
    /// </summary>
    private void UpdateStatusCheckCondition()
    {
        if (StatusType != eSceneEventStatusType.SceneEventCheck)
        {
            return;
        }
        if (CheckConditions())
        {
            StatusType = eSceneEventStatusType.SceneEventExecute;
            BroadcastSceneTriggerEventUpdate();
        }
    }
    /// <summary>
    /// 执行行为状态
    /// </summary>
    private void UpdateStatusExecuteAction()
    {
        if (StatusType != eSceneEventStatusType.SceneEventExecute)
        {
            return;
        }
        ExecuteActions();
        StatusType = eSceneEventStatusType.SceneEventFinish;
        BroadcastSceneTriggerEventUpdate();
    }

    /// <summary>
    /// 检查条件列表
    /// </summary>
    protected virtual bool CheckConditions()
    {
        if (Conditions.Count == 0)
        {
            return true;
        }

        int completeNum = 0;
        for (int i = 0; i < Conditions.Count; i++)
        {
            try
            {
                if (Conditions[i].Check())
                {
                    completeNum++;
                }
            }
            catch (Exception e)
            {
                Log.Error($"CheckCondition Error {e}");
            }

        }
        return CheckConditionType == eSTConditionCheckType.And ? completeNum == Conditions.Count : completeNum > 0;
    }

    /// <summary>
    /// 执行行为
    /// </summary>
    protected virtual void ExecuteActions()
    {
        if (Actions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < Actions.Count; i++)
        {
            try
            {
                Actions[i].Execute();
            }
            catch (Exception e)
            {
                Log.Error($"ExecuteAction Error {e}");
            }
        }
    }

    private void ClearCondition()
    {
        if (Conditions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < Conditions.Count; i++)
        {
            Conditions[i].Dispose();
        }
        Conditions.Clear();
    }

    private void ClearAction()
    {
        if (Actions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Dispose();
        }
        Actions.Clear();
    }

    public virtual void Clear()
    {
        ClearCondition();
        ClearAction();
        DRSceneEvent = null;
        Id = 0;
        StatusType = eSceneEventStatusType.SceneEventActivate;
        CheckConditionType = eSTConditionCheckType.And;
        EventType = eSTEventType.Main;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        StatusType = eSceneEventStatusType.SceneEventActivate;
    }


    public void Init(long id, int cid)
    {
        Id = id;
        DRSceneEvent = GFEntryCore.DataTable.GetDataTable<DRSceneEvent>().GetDataRow(cid);
        if (DRSceneEvent == null)
        {
            Log.Error($"InitSceneEvent Error: drSceneEvent is null cid = {cid}");
            return;
        }
        EventType = (eSTEventType)DRSceneEvent.Type;
        InitConditions();
        InitActions();
    }

    public void InitConditions()
    {
        if (DRSceneEvent.Conditions.Length == 0)
        {
            return;
        }

        for (int i = 0; i < DRSceneEvent.Conditions.Length; i++)
        {
            STConditionBase condition = GFEntryCore.SceneTriggerEventMgr.CreateSTCondition(DRSceneEvent.Conditions[i]);
            condition.SetSceneEvent(this);
            Conditions.Add(condition);
        }
    }

    public void InitActions()
    {
        if (DRSceneEvent.Actions.Length == 0)
        {
            return;
        }

        for (int i = 0; i < DRSceneEvent.Actions.Length; i++)
        {
            STActionBase action = GFEntryCore.SceneTriggerEventMgr.CreateSTAction(DRSceneEvent.Actions[i]);
            action.SetSceneEvent(this);
            Actions.Add(action);
        }
    }

    /// <summary>
    /// 创建场景触发器事件
    /// </summary>
    public static SceneTriggerEvent Create(Type eventClass)
    {
        SceneTriggerEvent sceneEvent = ReferencePool.Acquire(eventClass) as SceneTriggerEvent;
        return sceneEvent;
    }

    /// <summary>
    /// 销毁场景触发器事件
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }

    /// <summary>
    /// 广播场景事件数据更新，检测条件数据更新可以直接调用， 不用担心高频调用，有间隔时间合并广播
    /// </summary>
    public void BroadcastSceneTriggerEventUpdate()
    {
        GFEntryCore.SceneTriggerEventMgr.BroadcastSceneTriggerEventUpdate(GetNetData());
    }

    /// <summary>
    /// 广播执行行为，有些行为执行需要客户端感知
    /// </summary>
    public void BroadcastActionExecute(STActionBase actionBase)
    {
        GFEntryCore.SceneTriggerEventMgr.BroadcastSceneTriggerEventActionExecute(actionBase.DRSceneEventAction.Id);
    }

    /// <summary>
    /// 同步事件数据
    /// </summary>
    /// <param name="sceneEventData"></param> <summary>
    public void SyncNetData(SceneEventData sceneEventData)
    {
        StatusType = sceneEventData.Status;
        for (int i = 0; i < sceneEventData.ConditionDataList.Count; i++)
        {
            Conditions[i].SyncNetData(sceneEventData.ConditionDataList[i]);
        }

    }

    public SceneEventData GetNetData()
    {
        NetData.Id = Id;
        NetData.EventCid = DRSceneEvent.Id;
        NetData.Status = StatusType;
        NetData.ConditionDataList.Clear();
        for (int i = 0; i < Conditions.Count; i++)
        {
            NetData.ConditionDataList.Add(Conditions[i].GetNetData());
        }
        return NetData;
    }

}
