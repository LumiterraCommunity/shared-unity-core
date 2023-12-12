/*
 * @Author: xiang huan
 * @Date: 2023-10-19 10:33:56
 * @Description: 副本关卡状态触发器
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/Trigger/InstancingLevelStatusTrigger.cs
 * 
 */

using System;
using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;

public class InstancingLevelStatusTrigger : SharedCoreComponent
{

    public enum eOperation
    {
        And,        //与
        Or,         //或
    }
    [Serializable]
    public struct LevelStatusData
    {
        public int LevelIndex;
        public eInstancingStatusType StatusType;
    }

    [Header("关卡状态条件列表")]
    public List<LevelStatusData> StatusList = new();

    [Header("触发节点")]
    public GameObject TriggerNode;

    [Header("条件与或非")]
    public eOperation Operation = eOperation.And;

    private void Start()
    {
        MessageCore.LevelStatusUpdate += OnLevelStatusUpdate;
        MessageCore.InstancingLevelInitFinish += UpdateLevelStatus;
        UpdateLevelStatus();
    }

    private void OnDestroy()
    {
        MessageCore.LevelStatusUpdate -= OnLevelStatusUpdate;
        MessageCore.InstancingLevelInitFinish -= UpdateLevelStatus;
    }

    private void OnLevelStatusUpdate(int levelIndex, eInstancingStatusType statusType)
    {
        UpdateLevelStatus();
    }

    public void UpdateLevelStatus()
    {
        if (TriggerNode == null)
        {
            return;
        }

        if (!GFEntryCore.IsExistModule<IInstancingMgr>())
        {
            return;
        }

        IInstancingMgr instancingMgr = GFEntryCore.GetModule<IInstancingMgr>();
        if (!instancingMgr.IsInit)
        {
            return;
        }

        int succeedNum = 0;
        for (int i = 0; i < StatusList.Count; i++)
        {
            LevelStatusData levelStatusData = StatusList[i];

            eInstancingStatusType statusType = instancingMgr.GetLevelStatus(levelStatusData.LevelIndex);
            if (statusType == levelStatusData.StatusType)
            {
                succeedNum++;
            }
        }

        bool isActive = Operation == eOperation.And ? succeedNum == StatusList.Count : succeedNum > 0;
        TriggerNode.SetActive(isActive);
    }
}
