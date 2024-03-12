using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;

/// <summary>
/// 家园实体管理类
/// </summary>
/// <typeparam name="TFactory"></typeparam>
public class HomeEntityMgrCore<TFactory> : MonoBehaviour, IHomeEntityMgrCore where TFactory : HomeEntityFactoryCore, new()
{
    public readonly Dictionary<long, HomeEntityCore> EntityMap = new();

    private readonly TFactory _factory = new();

    public HomeEntityCore AddEntity(long id, SeedFunctionType type, GameObject root)
    {
        if (EntityMap.ContainsKey(id))
        {
            Debug.LogError($"HomeEntity AddEntity Error: Entity {id} already exists");
            RemoveEntity(id);
        }

        HomeEntityCore entity = _factory.CreateHomeEntity(root, type);
        entity.SetBaseInfo(id, type);
        EntityMap.Add(id, entity);
        return entity;
    }

    public void RemoveEntity(long id)
    {
        if (EntityMap.TryGetValue(id, out HomeEntityCore entity))
        {
            try
            {
                entity.Dispose();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"HomeEntity RemoveEntity Error: Entity dispose failed,id:{id} type:{entity.Type} error:{e}");
            }
            _ = EntityMap.Remove(id);
        }
        else
        {
            Debug.LogError($"HomeEntity RemoveEntity Error: Entity {id} not found");
        }
    }
}