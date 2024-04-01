using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 检测周围实体的雷达基类 利用碰撞盒检测周围实体
/// </summary>
public abstract class CheckEntityRadarBase : MonoBehaviour
{
    /// <summary>
    /// 监测半径 米
    /// </summary>
    /// <value></value>
    protected abstract float CheckRadius { get; }
    /// <summary>
    /// 监测的层mask
    /// </summary>
    /// <value></value>
    protected abstract LayerMask CheckLayer { get; }

    private GameObject _triggerRoot;//在加载本脚本的时候创建一个空物体，用来添加碰撞盒
    /// <summary>
    /// 附近的实体Id映射 key:实体Id
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<long, EntityBase> _entityIdMap = new();
    /// <summary>
    /// 附近的实体碰撞盒映射 key:collider的hash 主要为了TriggerExit时不用拿实体组件 提高效率
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<int, EntityBase> _entityColliderMap = new();

    protected virtual void Start()
    {
        _triggerRoot = GameObjectUtil.CreateGameObject(GetType().Name, transform);
        _triggerRoot.layer = MLayerMask.ENTITY_CHECK;

        SphereCollider collider = _triggerRoot.AddComponent<SphereCollider>();
        collider.radius = CheckRadius;
        collider.isTrigger = true;

        RadarHelper helper = _triggerRoot.AddComponent<RadarHelper>();
        helper.OnOneTriggerEnter += OnOneTriggerEnter;
        helper.OnOneTriggerExit += OnOneTriggerExit;
    }

    protected virtual void OnDestroy()
    {
        if (_triggerRoot != null)
        {
            Destroy(_triggerRoot);
            _triggerRoot = null;
        }

        foreach (EntityBase entity in _entityIdMap.Values)
        {
            entity.EntityEvent.UnInitFromScene -= RemoveEntity;
        }
        _entityIdMap.Clear();
        _entityColliderMap.Clear();
    }

    private void OnOneTriggerEnter(Collider other)
    {
        //不是指定层
        if ((CheckLayer & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        if (other.TryGetComponent(out EntityReferenceData entityReference))
        {
            AddEntity(entityReference.Entity, other);
        }
    }

    private void OnOneTriggerExit(Collider other)
    {
        if (_entityColliderMap.TryGetValue(other.GetHashCode(), out EntityBase entity))
        {
            RemoveEntity(entity);
        }
    }

    private void AddEntity(EntityBase entity, Collider collider)
    {
        if (_entityIdMap.ContainsKey(entity.BaseData.Id))
        {
            Log.Error($"CheckEntityRadarBase Entity already exists, id:{entity.BaseData.Id}");
            return;
        }

        _entityIdMap.Add(entity.BaseData.Id, entity);
        _entityColliderMap.Add(collider.GetHashCode(), entity);

        entity.EntityEvent.UnInitFromScene += RemoveEntity;

        try
        {
            OnAddEntity(entity);
        }
        catch (System.Exception e)
        {
            Log.Error($"CheckEntityRadarBase OnAddEntity error, id:{entity.BaseData.Id},error:{e}");
        }
    }

    private void RemoveEntity(EntityBase entity)
    {
        if (!_entityIdMap.Remove(entity.BaseData.Id))
        {
            Log.Error($"CheckEntityRadarBase Entity not found, id:{entity.BaseData.Id}");
            return;
        }

        try
        {
            OnRemoveEntity(entity);
        }
        catch (System.Exception e)
        {
            Log.Error($"CheckEntityRadarBase OnRemoveEntity error, id:{entity.BaseData.Id},error:{e}");
        }

        EntityCollisionCore entityCollisionCore = entity.GetComponent<EntityCollisionCore>();
        if (!_entityColliderMap.Remove(entityCollisionCore.BodyCollision.GetHashCode()))
        {
            Log.Error($"CheckEntityRadarBase Entity collider not found, id:{entity.BaseData.Id}");
        }

        entity.EntityEvent.UnInitFromScene -= RemoveEntity;
    }

    /// <summary>
    /// 成功添加附近实体
    /// </summary>
    /// <param name="entity"></param>
    protected abstract void OnAddEntity(EntityBase entity);

    /// <summary>
    /// 成功移除附近实体
    /// </summary>
    /// <param name="entity"></param>
    protected abstract void OnRemoveEntity(EntityBase entity);

    /// <summary>
    /// 启用或者关闭雷达 关闭后触发器会隐藏
    /// </summary>
    /// <param name="active"></param>
    public void SetEnable(bool active)
    {
        if (enabled == active)
        {
            return;
        }
        enabled = active;

        if (_triggerRoot != null)
        {
            _triggerRoot.SetActive(active);
        }
    }
}