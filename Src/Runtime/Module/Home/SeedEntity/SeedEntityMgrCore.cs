using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;

/// <summary>
/// 种子实体管理类
/// </summary>
/// <typeparam name="TFactory"></typeparam>
public class SeedEntityMgrCore<TFactory> : MonoBehaviour, ISeedEntityMgrCore where TFactory : SeedEntityFactoryCore, new()
{
    public readonly Dictionary<long, SeedEntityCore> EntityMap = new();
    public readonly Dictionary<SeedFunctionType, List<SeedEntityCore>> TypeMap = new();

    private readonly TFactory _factory = new();

    /// <summary>
    /// 确保实体类型在map中存在List分配
    /// </summary>
    /// <param name="type"></param>
    private void EnsureEntityType(SeedFunctionType type)
    {
        if (!TypeMap.ContainsKey(type))
        {
            TypeMap.Add(type, new());
        }
    }

    public List<SeedEntityCore> GetEntities(SeedFunctionType type)
    {
        EnsureEntityType(type);

        return TypeMap[type];
    }

    public SeedEntityCore AddEntity(long id, SeedFunctionType type, GameObject root)
    {
        if (EntityMap.ContainsKey(id))
        {
            Debug.LogError($"SeedEntity AddEntity Error: Entity {id} already exists");
            RemoveEntity(id);
        }

        SeedEntityCore entity = _factory.CreateSeedEntity(root, type);
        entity.SetBaseInfo(id, type);

        EntityMap.Add(id, entity);

        EnsureEntityType(type);
        TypeMap[type].Add(entity);

        return entity;
    }

    public void RemoveEntity(long id)
    {
        if (EntityMap.TryGetValue(id, out SeedEntityCore entity))
        {
            SeedFunctionType type = entity.Type;

            try
            {
                entity.Dispose();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"SeedEntity RemoveEntity Error: Entity dispose failed,id:{id} type:{entity.Type} error:{e}");
            }

            _ = TypeMap[type].Remove(entity);
            _ = EntityMap.Remove(id);
        }
        else
        {
            Debug.LogError($"SeedEntity RemoveEntity Error: Entity {id} not found");
        }
    }

    public bool TryGetEntity(long id, out SeedEntityCore entity)
    {
        return EntityMap.TryGetValue(id, out entity);
    }

    public bool HasEntity(long id)
    {
        return EntityMap.ContainsKey(id);
    }
}