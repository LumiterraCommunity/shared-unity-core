using System;


public interface IEntityMgr
{
    T GetEntityWithRoot<T>(UnityEngine.GameObject go) where T : EntityBase;
    T GetEntity<T>(long id) where T : EntityBase;
    /// <summary>
    /// 删除一个实体
    /// </summary>
    /// <param name="entityID"></param>
    void RemoveEntity(long entityID);

    bool TryGetEntity(long id, out EntityBase entity);

    /// <summary>
    /// 遍历某个类型的所有实体 返回类型实体数量
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="elementCB">元素处理回调 返回true为需要打断遍历</param>
    /// <returns></returns>
    int ForeachTypeEntity(GameMessageCore.EntityType entityType, Func<EntityBase, bool> elementCB);
}