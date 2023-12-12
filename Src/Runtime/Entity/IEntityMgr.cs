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
}