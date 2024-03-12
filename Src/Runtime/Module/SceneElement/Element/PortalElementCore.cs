/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 传送门组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/PortalElementCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using GameMessageCore;

public class PortalElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.Portal;
    [SerializeField]
    public struct PortalTypeInfo
    {
        public ePortalType PortalType;
        public UnityEngine.Vector3 Pos;
        public int Weight;
    }
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

    [Header("当前使用次数")]
    public int CurUseNum = 0;

    [Header("最大使用次数")]
    public int MaxUseNum = 0;

    private long _startTime = long.MaxValue;
    private float _curActivateTime = 0;
    private readonly List<Collider> _playerList = new();
    private readonly HashSet<EntityBase> _portalDic = new();

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
        if (_playerList.Count > 0)
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
        if (_playerList.Count == 0)
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
        for (int i = 0; i < _playerList.Count; i++)
        {
            EntityBase entity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(_playerList[i].gameObject);
            if (entity != null && entity.Inited && entity.BattleDataCore.IsLive() && !_portalDic.Contains(entity))
            {
                CurUseNum++;
                _ = _portalDic.Add(entity);
                entity.EntityEvent.EntityTriggerPortalElement?.Invoke(this);
            }

            if (CurUseNum >= MaxUseNum)
            {
                break;
            }
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
        if (entity == null || !entity.Inited || !entity.BattleDataCore.IsLive())
        {
            return;
        }
        _playerList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }
        _ = _playerList.Remove(other);

    }

}
