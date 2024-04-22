/*
 * @Author: xiang huan
 * @Date: 2022-10-09 10:43:29
 * @Description: 实体传送组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/EntityPortalCore.cs
 * 
 */
using UnityEngine;

/// <summary>
/// 实体传送组件
/// </summary>
public class EntityPortalCore : EntityBaseComponent
{
    protected PortalElementCore PortalElement;
    protected ePortalStatusType StatusType;
    protected float WaitTriggerTime;
    protected virtual void Start()
    {
        RefEntity.EntityEvent.EnterPortalElement += EnterPortalElement;
        RefEntity.EntityEvent.ExitPortalElement += ExitPortalElement;
    }

    protected virtual void OnDestroy()
    {
        RefEntity.EntityEvent.EnterPortalElement -= EnterPortalElement;
        RefEntity.EntityEvent.ExitPortalElement -= ExitPortalElement;
    }

    protected virtual void Update()
    {
        if (PortalElement == null)
        {
            return;
        }
        UpdateStatusActivate();
        UpdateStatusRunning();
    }

    protected virtual void UpdateStatusActivate()
    {
        if (StatusType != ePortalStatusType.Activate)
        {
            return;
        }

        if (!CheckIsTrigger())
        {
            return;
        }
        StartRunning();
    }

    protected virtual void UpdateStatusRunning()
    {
        if (StatusType != ePortalStatusType.Running)
        {
            return;
        }

        if (!CheckIsTrigger())
        {
            StopRunning();
            return;
        }

        WaitTriggerTime -= Time.deltaTime;
        if (WaitTriggerTime <= 0)
        {
            TriggerPortalElement();
            StopRunning(ePortalStatusType.Finish);
            return;
        }
        UpdateRunning();
    }

    private void EnterPortalElement(PortalElementCore portalElementCore)
    {
        if (PortalElement != null)
        {
            return;
        }
        PortalElement = portalElementCore;
    }

    private void ExitPortalElement(PortalElementCore portalElementCore)
    {
        if (PortalElement != portalElementCore)
        {
            return;
        }
        PortalElement = null;
        StopRunning();
    }
    protected virtual void TriggerPortalElement()
    {

    }

    private bool CheckIsTrigger()
    {
        if (PortalElement == null || PortalElement.StatusType != ePortalStatusType.Running)
        {
            return false;
        }
        if (PortalElement.CurUseNum >= PortalElement.MaxUseNum)
        {
            return false;
        }
        //非活着的实体不触发  
        if (!RefEntity.BattleDataCore.IsLive())
        {
            return false;
        }
        return true;
    }

    protected virtual void StartRunning()
    {
        WaitTriggerTime = PortalElement.TriggerPortalTime;
        StatusType = ePortalStatusType.Running;
    }
    protected virtual void StopRunning(ePortalStatusType nextStatus = ePortalStatusType.Activate)
    {
        WaitTriggerTime = 0;
        StatusType = nextStatus;
    }

    protected virtual void UpdateRunning()
    {

    }
}