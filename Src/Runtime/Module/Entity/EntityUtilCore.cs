using GameMessageCore;


/// <summary>
/// 实体工具类
/// </summary>
public static class EntityUtilCore
{
    /// <summary>
    /// 判断是玩家类型 主要是由于客户端主角会用到MainPlayer类型
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static bool EntityTypeIsPlayer(EntityType entityType)
    {
        return entityType == EntityType.Player || entityType == EntityType.MainPlayer;
    }
}