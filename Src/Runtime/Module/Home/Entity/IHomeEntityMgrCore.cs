using GameMessageCore;
using UnityEngine;


/// <summary>
/// 家园实体管理类接口
/// </summary>
public interface IHomeEntityMgrCore
{
    /// <summary>
    /// 添加一个新的家园实体 会自动创建 但是还不会具体初始化逻辑
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="root">本实体脚本挂载的节点</param>
    /// <returns></returns>
    HomeEntityCore AddEntity(long id, SeedFunctionType type, GameObject root);

    /// <summary>
    /// 移除一个家园实体 会自动销毁
    /// </summary>
    /// <param name="id"></param>
    void RemoveEntity(long id);
}