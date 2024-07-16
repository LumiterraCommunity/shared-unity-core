/*
 * @Author: xiang huan
 * @Date: 2022-08-26 14:25:46
 * @Description: 实体层碰撞触发组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityCollisionTriggerCore.cs
 * 
 */
using CMF;
using UnityEngine;
using System.Collections.Generic;

public class EntityCollisionTriggerCore : MonoBehaviour
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
        if (other.TryGetComponent(out EntityReferenceData entityReference))
        {
            if (!EntityTriggerSet.Contains(entityReference.Entity.BaseData.Id))
            {
                _ = EntityTriggerSet.Add(entityReference.Entity.BaseData.Id);
            }
            RefEntity.EntityEvent.EntityTriggerEnter?.Invoke(entityReference.Entity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EntityReferenceData entityReference))
        {
            if (EntityTriggerSet.Contains(entityReference.Entity.BaseData.Id))
            {
                _ = EntityTriggerSet.Remove(entityReference.Entity.BaseData.Id);
            }
            RefEntity.EntityEvent.EntityTriggerExit?.Invoke(entityReference.Entity.BaseData.Id);
        }
    }

    public void Clear()
    {
        EntityTriggerSet.Clear();
    }
}