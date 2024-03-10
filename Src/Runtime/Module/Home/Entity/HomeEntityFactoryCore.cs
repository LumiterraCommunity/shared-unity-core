using UnityEngine;

/// <summary>
/// 家园实体工厂
/// </summary>
public class HomeEntityFactoryCore<TEntity> where TEntity : HomeEntityCore, new()
{
    /// <summary>
    /// 创建一个家园实体(挂载组件)
    /// </summary>
    /// <param name="root">挂载的root</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public TEntity CreateHomeEntity(GameObject root, GameMessageCore.SeedFunctionType type)
    {
        TEntity entity = new();
        return AssemblyEntity(entity);//返回装备后的实体
    }

    /// <summary>
    /// 根据实体类型装配出不同feature的实体
    /// </summary>
    /// <param name="entity"></param>
    protected virtual TEntity AssemblyEntity(TEntity entity)
    {
        return entity;
    }
}