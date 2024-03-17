using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;


/// <summary>
/// 种子实体管理类接口
/// </summary>
public interface ISeedEntityMgrCore
{
    /// <summary>
    /// 获取指定类型的所有实体 不会为null 不会GC 不要改内部内容
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    List<SeedEntityCore> GetEntities(SeedFunctionType type);

    /// <summary>
    /// 添加一个新的种子实体 会自动创建 但是还不会具体初始化逻辑
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="root">本实体脚本挂载的节点</param>
    /// <returns></returns>
    SeedEntityCore AddEntity(long id, SeedFunctionType type, GameObject root);

    /// <summary>
    /// 移除一个种子实体 会自动销毁释放 土地也会回归到初始状态
    /// </summary>
    /// <param name="id"></param>
    void RemoveEntity(long id);
    /// <summary>
    /// 尝试获取一个实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool TryGetEntity(long id, out SeedEntityCore entity);
    bool HasEntity(long id);
}