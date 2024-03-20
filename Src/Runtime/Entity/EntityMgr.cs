using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EntityMgr<TEntity, TFactory> : SceneModuleBase, IEntityMgr where TEntity : EntityBase, new() where TFactory : EntityFactory<TEntity>, new()
{
    /// <summary>
    /// 场景所有实体 包括了主角
    /// </summary>
    /// <returns></returns>
    protected readonly Dictionary<long, TEntity> EntityDic = new();
    /// <summary>
    /// 场景所有实体的归类字典 通过type作为key，每个type下面又有一个字典，通过id作为key
    /// </summary>
    /// <returns></returns>
    protected readonly Dictionary<GameMessageCore.EntityType, Dictionary<long, TEntity>> EntityTypeDic = new();
    /// <summary>
    /// 场景所有实体 包括了主角,通过root节点id作为key
    /// </summary>
    /// <returns></returns>
    protected readonly Dictionary<int, TEntity> EntityRootDic = new();
    /// <summary>
    /// 实体工厂，用于创建实体
    /// </summary>
    protected TFactory Factory = new();

    /// <summary>
    /// 所有实体数量
    /// </summary>
    public int EntityCount => EntityDic.Count;

    /// <summary>
    /// 获取存在的场景实体 找不到会报错 返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns>如果没有会返回null</returns>
    public TEntity GetEntity(long id)
    {
        //EntityBase是个复杂对象的时候，可以用这个方法来获取效率会更高，有待观察 https://blog.csdn.net/sigmeta/article/details/121534293
        // if (EntityDic.ContainsKey(id))
        // {
        //     return EntityDic[id];
        // }

        if (TryGetEntity(id, out TEntity entity))
        {
            return entity;
        }

        Log.Error($"Can not find entity with id {id}");
        return null;
    }

    /// <summary>
    /// 尝试获取存在的场景实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool TryGetEntity(long id, out TEntity entity)
    {
        return EntityDic.TryGetValue(id, out entity);
    }

    public T GetEntity<T>(long id) where T : EntityBase
    {
        return GetEntity(id) as T;
    }

    public bool TryGetEntity(long id, out EntityBase entity)
    {
        if (EntityDic.TryGetValue(id, out TEntity tEntity))
        {
            entity = tEntity;
            return true;
        }
        entity = null;
        return false;
    }

    public bool HasEntity(long id)
    {
        return EntityDic.ContainsKey(id);
    }

    /// <summary>
    /// 通过实体显示节点反查逻辑实体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public TEntity GetEntityWithRoot(GameObject go)
    {
        return GetEntityWithRoot<TEntity>(go);
    }

    /// <summary>
    /// 运行函数时传入范型获取实体
    /// </summary>
    /// <param name="go"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetEntityWithRoot<T>(GameObject go) where T : EntityBase
    {
        int goID = go.GetInstanceID();
        if (EntityRootDic.ContainsKey(goID))
        {
            return EntityRootDic[goID] as T;
        }

        Log.Warning($"Can not find entity with root, name: {go.name}, id: {goID}");
        return null;
    }

    /// <summary>
    /// 获取所有实体 不走GC 不要改变里面值 而且不要频繁使用 慎用
    /// </summary>
    /// <returns></returns>
    public Dictionary<long, TEntity> GetAllEntityNoGC()
    {
        return EntityDic;
    }

    /// <summary>
    /// 获取entityType类型的所有实体 不走GC 不要改变里面值 慎用
    /// 返回值一定不为空
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public Dictionary<long, TEntity> GetAllEntityOfType(GameMessageCore.EntityType entityType)
    {
        if (EntityTypeDic.TryGetValue(entityType, out Dictionary<long, TEntity> entityDic))
        {
            return entityDic;
        }

        entityDic = new();
        EntityTypeDic.Add(entityType, entityDic);
        return entityDic;
    }

    /// <summary>
    /// 添加一个场景实体 主角使用另外一个方法
    /// </summary>
    /// <param name="entityID"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public virtual TEntity AddEntity(long entityID, GameMessageCore.EntityType entityType)
    {
        if (EntityDic.ContainsKey(entityID))
        {
            Log.Error($"Entity {entityID} already exist,type={entityType}");
            RemoveEntity(entityID);
        }

        try
        {
            TEntity entity = CreateEntity(entityID, entityType);
            AddEntityToDataCache(entity);
            return entity;
        }
        catch (Exception e)
        {
            Log.Error($"Entity {entityID} init failed,type={entityType},error={e}");
            return null;
        }
    }

    /// <summary>
    /// 移除一个场景实体 主角不使用这个方法
    /// </summary>
    /// <param name="entityID"></param>
    public virtual void RemoveEntity(long entityID)
    {
        if (!EntityDic.TryGetValue(entityID, out TEntity entity))
        {
            Log.Error($"Entity {entityID} not exist");
            return;
        }

        RemoveEntityFromDataCache(entityID);
        try
        {
            DisposeEntity(entity);
        }
        catch (Exception e)
        {
            Log.Error($"Entity {entityID} dispose failed,error={e}");
        }
    }

    public bool TryRemoveEntity(long entityID)
    {
        if (!HasEntity(entityID))
        {
            return false;
        }

        RemoveEntity(entityID);
        return true;
    }

    /// <summary>
    /// 移除所有实体
    /// </summary>
    public virtual void RemoveAllEntity()
    {
        RemoveAllEntityExcept(null);
    }

    /// <summary>
    /// 移除除了exceptIds之外的所有实体
    /// </summary>
    /// <param name="retainIds"></param>
    public virtual void RemoveAllEntityExcept(IEnumerable<long> retainIds)
    {
        List<TEntity> retainEntities = new();
        HashSet<long> retainIdSet;
        retainIdSet = retainIds != null ? new(retainIds) : null;
        foreach (KeyValuePair<long, TEntity> item in EntityDic)
        {
            if (retainIdSet != null && retainIdSet.Contains(item.Key))
            {
                retainEntities.Add(item.Value);
                continue;
            }
            try
            {
                DisposeEntity(item.Value);
            }
            catch (Exception e)
            {
                Log.Error($"Entity {item.Value.BaseData.Id} RemoveAllEntityExcept dispose failed,error={e}");
            }
        }

        EntityDic.Clear();
        EntityRootDic.Clear();
        EntityTypeDic.Clear();

        foreach (TEntity entity in retainEntities)
        {
            AddEntityToDataCache(entity);
        }
    }

    /// <summary>
    /// 释放一个实体，内部使用
    /// 这里只是销毁实体，不包含移除逻辑，外部想要移除实体请使用RemoveEntity
    /// </summary>
    /// <param name="entity"></param>
    protected virtual void DisposeEntity(EntityBase entity)
    {
        if (entity == null)
        {
            return;
        }

        entity.Dispose();
    }

    protected virtual TEntity CreateEntity(long entityID, GameMessageCore.EntityType entityType)
    {
        return Factory.CreateSceneEntity(entityID, entityType);
    }

    public override void UnloadSceneBefore()
    {
        RemoveAllEntity();
    }

    /// <summary>
    /// 统一把实体添加到数据缓存中，所有缓存实体的数据结构都在这里添加数据，譬如 Dictionary<Type,List<TEntity>>等
    /// </summary>
    /// <param name="entity"></param>
    protected virtual void AddEntityToDataCache(TEntity entity)
    {
        EntityDic.Add(entity.BaseData.Id, entity);
        EntityRootDic.Add(entity.RootID, entity);
        Dictionary<long, TEntity> dic = GetAllEntityOfType(entity.BaseData.Type);
        dic.Add(entity.BaseData.Id, entity);
    }

    /// <summary>
    /// 统一移除数据缓存入口，所有缓存实体的数据结构都在这里移除数据
    /// </summary>
    /// <param name="id"></param>
    protected virtual void RemoveEntityFromDataCache(long id)
    {
        if (!EntityDic.TryGetValue(id, out TEntity entity))
        {
            return;
        }

        _ = EntityDic.Remove(id);
        _ = EntityRootDic.Remove(entity.RootID);
        if (EntityTypeDic.TryGetValue(entity.BaseData.Type, out Dictionary<long, TEntity> dic))
        {
            _ = dic.Remove(id);
        }
    }
}