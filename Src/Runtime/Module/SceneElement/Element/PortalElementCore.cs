/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 传送门组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/PortalElementCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using GameMessageCore;
using System;

public class PortalElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.Portal;
    [Serializable]
    public struct PortalTypeInfo
    {
        public ePortalType PortalType;
        public int Weight;
    }

    [Header("传送位置")]
    public UnityEngine.Vector3 TransferPos;

    [Header("传送门类型列表")]
    public List<PortalTypeInfo> PortalTypeList;

    [Header("当前传送门索引")]
    public int CurTypeIndex;

    [Header("传送门状态")]
    public ePortalStatusType StatusType = ePortalStatusType.Inactive;
    [Header("传送门特效")]
    public GameObject PortalElementEffect;

    [Header("传送门激活时间(s)")]
    public float ActivateTime = 0;

    [Header("传送门传送时间(s)")]
    public float TriggerPortalTime = 3;

    [Header("当前使用次数")]
    public int CurUseNum = 0;

    [Header("最大使用次数")]
    public int MaxUseNum = 0;

    private long _startTime = long.MaxValue;
    private float _curActivateTime = 0;
    private readonly ListMap<Collider, EntityBase> _playerList = new();
    protected override void Update()
    {
        base.Update();
        UpdateStatusHide();
        UpdateStatusInactive();
        UpdateStatusActivate(Time.deltaTime);
        UpdateStatusRunning();
        if (PortalElementEffect != null)
        {
            PortalElementEffect.SetActive(StatusType != ePortalStatusType.Hide);
        }
    }
    public override void UpdateElementData()
    {
        base.UpdateElementData();
        PortalElementData netData = new()
        {
            StartTime = _startTime,
            StatusType = (int)StatusType,
            CurUseNum = CurUseNum,
            CurTypeIndex = CurTypeIndex,
        };
        SceneElementData.Portal = netData;
    }

    public void StartElement(long startTime, ePortalStatusType statusType, int curUseNum, int curTypeIndex)
    {
        _startTime = startTime;
        StatusType = statusType;
        CurUseNum = curUseNum;
        CurTypeIndex = curTypeIndex;
        UpdateElementData();
    }

    public override void InitElementData(SceneElementData netData)
    {
        PortalElementData portal = netData.Portal;
        StartElement(portal.StartTime, (ePortalStatusType)portal.StatusType, portal.CurUseNum, portal.CurTypeIndex);
    }

    private void UpdateStatusHide()
    {
        if (StatusType != ePortalStatusType.Hide)
        {
            return;
        }
        long curTime = TimeUtil.GetServerTimeStamp();
        if (curTime < _startTime)
        {
            return;
        }
        StatusType = ePortalStatusType.Inactive;
    }
    private void UpdateStatusInactive()
    {
        if (StatusType != ePortalStatusType.Inactive)
        {
            return;
        }
        if (CheckHasActivate())
        {
            StatusType = ePortalStatusType.Activate;
            _curActivateTime = 0;
        }
    }


    private void UpdateStatusActivate(float deltaTime)
    {
        if (StatusType != ePortalStatusType.Activate)
        {
            return;
        }
        if (!CheckHasActivate())
        {
            StatusType = ePortalStatusType.Inactive;
        }
        else
        {
            _curActivateTime += deltaTime;
            if (_curActivateTime >= ActivateTime)
            {
                StatusType = ePortalStatusType.Running;
            }
        }
    }

    private void UpdateStatusRunning()
    {
        if (StatusType != ePortalStatusType.Running)
        {
            return;
        }

        if (CurUseNum >= MaxUseNum)
        {
            StatusType = ePortalStatusType.Finish;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }
        EntityBase entity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(other.gameObject);
        if (entity == null)
        {
            return;
        }
        entity.EntityEvent.EnterPortalElement?.Invoke(this);
        _ = _playerList.Add(other, entity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }
        if (_playerList.TryGetValueFromKey(other, out EntityBase entity))
        {
            _ = _playerList.Remove(other);
            entity.EntityEvent.ExitPortalElement?.Invoke(this);
        }
    }

    private bool CheckHasActivate()
    {
        if (_playerList.Count == 0)
        {
            return false;
        }
        EntityBase topEntity = null;
        for (int i = 0; i < _playerList.Count; i++)
        {
            EntityBase entity = _playerList[i];
            if (entity.Inited && entity.BattleDataCore.IsLive())
            {
                if (topEntity == null)
                {
                    topEntity = entity;
                }
                else
                {
                    // 有敌人在传送门内
                    if (topEntity.EntityCampDataCore.CheckIsEnemy(entity))
                    {
                        return false;
                    }
                }
            }
        }
        return topEntity != null;
    }

    public void TriggerPortalElement(EntityBase entityBase)
    {
        CurUseNum++;
    }
}
