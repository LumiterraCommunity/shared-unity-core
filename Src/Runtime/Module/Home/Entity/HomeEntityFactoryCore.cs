using UnityEngine;

/// <summary>
/// 家园实体工厂
/// </summary>
public abstract class HomeEntityFactoryCore
{
    /// <summary>
    /// 创建一个家园实体(挂载组件)
    /// </summary>
    /// <param name="root">挂载的root</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public abstract HomeEntityCore CreateHomeEntity(GameObject root, GameMessageCore.SeedFunctionType type);
}