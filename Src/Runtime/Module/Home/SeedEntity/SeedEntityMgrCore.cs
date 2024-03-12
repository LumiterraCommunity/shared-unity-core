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

    private readonly TFactory _factory = new();

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
        return entity;
    }

    public void RemoveEntity(long id)
    {
        if (EntityMap.TryGetValue(id, out SeedEntityCore entity))
        {
            try
            {
                entity.Dispose();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"SeedEntity RemoveEntity Error: Entity dispose failed,id:{id} type:{entity.Type} error:{e}");
            }
            _ = EntityMap.Remove(id);
        }
        else
        {
            Debug.LogError($"SeedEntity RemoveEntity Error: Entity {id} not found");
        }
    }
}