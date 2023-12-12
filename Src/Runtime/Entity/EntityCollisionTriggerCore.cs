/*
 * @Author: xiang huan
 * @Date: 2022-08-26 14:25:46
 * @Description: 实体层碰撞触发组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/HotFix/Entity/EntityCollisionTriggerCore.cs
 * 
 */
using CMF;
using UnityEngine;
using System.Collections.Generic;

public class EntityCollisionTrigger : MonoBehaviour
{
    public EntityBase RefEntity { get; private set; }
    public long EntityId { get; private set; }
    public void Init(EntityBase entity)
    {
        RefEntity = entity;
        EntityId = entity.BaseData.Id;
    }
    public HashSet<long> EntityTriggerSet = new(); //触碰实体Map
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & (1 << MLayerMask.ENTITY_TRIGGER)) == 0)
        {
            return;
        }
        if (other.TryGetComponent(out EntityCollisionTrigger trigger) && trigger.RefEntity != null && trigger.RefEntity.Inited)
        {
            if (!EntityTriggerSet.Contains(trigger.EntityId))
            {
                _ = EntityTriggerSet.Add(trigger.EntityId);
            }
            RefEntity.EntityEvent.EntityTriggerEnter?.Invoke(trigger.RefEntity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & (1 << MLayerMask.ENTITY_TRIGGER)) <= 0)
        {
            return;
        }
        if (other.TryGetComponent(out EntityCollisionTrigger trigger))
        {
            _ = EntityTriggerSet.Remove(trigger.EntityId);
            RefEntity.EntityEvent.EntityTriggerExit?.Invoke(trigger.EntityId);
        }
    }
}