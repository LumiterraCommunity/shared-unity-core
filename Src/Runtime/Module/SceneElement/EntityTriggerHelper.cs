/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 触发元素通用基类
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/EntityTriggerHelper.cs
 * 
 */
using System;
using UnityEngine;

public class EntityTriggerHelper : SharedCoreComponent
{
    public readonly ListMap<Collider, EntityBase> EntityDic = new();
    public event Action<Collider, EntityBase> OnAddEntity;
    public event Action<Collider, EntityBase> OnRemoveEntity;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out EntityReferenceData refData))
        {
            return;
        }
        AddEntity(other, refData.Entity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out EntityReferenceData refData))
        {
            return;
        }
        RemoveEntity(other);
    }

    protected virtual void AddEntity(Collider other, EntityBase entity)
    {
        if (EntityDic.ContainsKey(other))
        {
            return;
        }
        _ = EntityDic.Add(other, entity);

        OnAddEntity?.Invoke(other, entity);
    }

    protected virtual void RemoveEntity(Collider other)
    {
        if (EntityDic.TryGetValueFromKey(other, out EntityBase entity))
        {
            _ = EntityDic.Remove(other);
            OnRemoveEntity?.Invoke(other, entity);
        }
    }
}
