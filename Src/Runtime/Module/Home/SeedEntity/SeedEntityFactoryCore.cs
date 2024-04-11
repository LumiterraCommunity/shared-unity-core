using UnityEngine;

/// <summary>
/// 种子实体工厂
/// </summary>
public abstract class SeedEntityFactoryCore
{
    /// <summary>
    /// 创建一个种子实体(挂载组件)
    /// </summary>
    /// <param name="root">挂载的root</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public abstract SeedEntityCore CreateSeedEntity(GameObject root, GameMessageCore.SeedFunctionType type);
}