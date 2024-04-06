using GameMessageCore;
using UnityGameFramework.Runtime;


/// <summary>
/// 实体工具类
/// </summary>
public static class EntityUtilCore
{
    /// <summary>
    /// 判断实体类型是玩家单位
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static bool EntityTypeIsPlayerUnit(EntityType entityType)
    {
        return entityType is EntityType.Player or EntityType.MainPlayer or EntityType.Pet or EntityType.HomeAnimal;
    }

    /// <summary>
    /// 从实体单位上获取玩家id 不是玩家单位或者异常返回0 宠物返回主人 家园守卫返回家园主人 玩家返回自己
    /// </summary>
    /// <param name="entityBase"></param>
    /// <returns></returns>
    public static long GetPlayerIdFromEntityUnit(EntityBase entityBase)
    {
        if (entityBase == null)
        {
            return 0;
        }

        EntityType entityType = entityBase.BaseData.Type;
        if (!EntityTypeIsPlayerUnit(entityType))
        {
            return 0;
        }

        if (entityType is EntityType.Player or EntityType.MainPlayer)
        {
            return entityBase.BaseData.Id;
        }

        if ((entityType is EntityType.Pet or EntityType.HomeAnimal) && entityBase.TryGetComponent(out PetDataCore petDataCore))
        {
            return petDataCore.OwnerId;
        }

        //家园守卫待补充

        Log.Error($"GetPlayerIdFromEntityUnit,entityType:{entityType} id:{entityBase.BaseData.Id}");
        return 0;
    }

    /// <summary>
    /// 判断玩家能否正常离线
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static bool CheckPlayerCanOffline(EntityBase entityBase, SceneServiceSubType sceneType)
    {
        //副本场景
        if (sceneType == SceneServiceSubType.Dungeon)
        {
            return false;
        }

        //不是和平区域
        if (entityBase.TryGetComponent(out EntityBattleArea entityBattleArea) && entityBattleArea.CurAreaType != eBattleAreaType.Peace)
        {
            return false;
        }
        return true;
    }
}