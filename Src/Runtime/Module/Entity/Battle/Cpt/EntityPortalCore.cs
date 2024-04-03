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

    private void UpdateStatusActivate()
    {
        if (StatusType != ePortalStatusType.Activate)
        {
            return;
        }

        if (!CheckIsTrigger())
        {
            return;
        }

        WaitTriggerTime = PortalElement.TriggerPortalTime;
        StatusType = ePortalStatusType.Running;
    }

    private void UpdateStatusRunning()
    {
        if (StatusType != ePortalStatusType.Running)
        {
            return;
        }

        if (!CheckIsTrigger())
        {
            StatusType = ePortalStatusType.Activate;
            return;
        }

        WaitTriggerTime -= Time.deltaTime;
        if (WaitTriggerTime <= 0)
        {
            TriggerPortalElement();
            WaitTriggerTime = 0;
            StatusType = ePortalStatusType.Finish;
        }
    }
    private void EnterPortalElement(PortalElementCore portalElementCore)
    {
        if (PortalElement != null)
        {
            return;
        }
        PortalElement = portalElementCore;
        StatusType = ePortalStatusType.Activate;
    }

    private void ExitPortalElement(PortalElementCore portalElementCore)
    {
        if (PortalElement != portalElementCore)
        {
            return;
        }
        PortalElement = null;
        StatusType = ePortalStatusType.Activate;
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
        return true;
    }
}