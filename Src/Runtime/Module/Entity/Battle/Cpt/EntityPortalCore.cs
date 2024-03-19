/*
 * @Author: xiang huan
 * @Date: 2022-10-09 10:43:29
 * @Description: 实体传送组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/EntityPortalCore.cs
 * 
 */
using UnityEngine;

/// <summary>
/// 实体传送组件
/// </summary>
public class EntityPortalCore : EntityBaseComponent
{
    protected PortalElementCore PortalElement;
    protected bool IsTrigger;
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

        if (IsTrigger)
        {
            if (CheckStartTrigger())
            {
                WaitTriggerTime -= Time.deltaTime;
                if (WaitTriggerTime <= 0)
                {
                    TriggerPortalElement();
                    EndTrigger();

                }
            }
            else
            {
                EndTrigger();
            }
        }
        else
        {
            if (CheckStartTrigger())
            {
                StartTrigger();
            }
        }
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
        EndTrigger();
    }
    protected virtual void StartTrigger()
    {
        WaitTriggerTime = PortalElement.TriggerPortalTime;
        IsTrigger = true;
    }

    protected virtual void EndTrigger()
    {
        WaitTriggerTime = 0;
        IsTrigger = false;
    }

    protected virtual void TriggerPortalElement()
    {
        PortalElement.TriggerPortalElement(RefEntity);
    }

    private bool CheckStartTrigger()
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