using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 玩家身上击杀统计数据 当前是统计当日当前场景的
/// </summary>
public class PlayerKillDataCore : EntityBaseComponent
{
    private readonly Dictionary<EntityType, int> _killEntityNum = new();

    /// <summary>
    /// 获取某种实体的击杀数量
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public int GetEntityKillNum(EntityType entityType)
    {
        return _killEntityNum.TryGetValue(entityType, out int num) ? num : 0;
    }

    /// <summary>
    /// 从网络重设所有的击杀数量
    /// </summary>
    /// <param name="killCount"></param>
    public void SetAllFromNet(UserMapKillCount killCount)
    {
        _killEntityNum.Clear();

        if (killCount == null)
        {
            Log.Error("SetAllFromNet Error: killCount is null");
            return;
        }

        SetEntityKillNum(EntityType.Monster, killCount.MonsterKillCount);
        SetEntityKillNum(EntityType.Resource, killCount.ResourceKillCount);
    }

    /// <summary>
    /// 直接设置击杀数量
    /// </summary>
    public void SetEntityKillNum(EntityType entityType, int num)
    {
        _killEntityNum[entityType] = num;
    }

    /// <summary>
    /// 添加击杀数量 默认+1
    /// </summary>
    public void AddEntityKillNum(EntityType entityType, int num = 1)
    {
        if (num < 1)
        {
            Log.Error($"AddEntityKillNum Error: num = {num}");
            return;
        }

        if (_killEntityNum.TryGetValue(entityType, out int curNum))
        {
            _killEntityNum[entityType] = curNum + num;
        }
        else
        {
            _killEntityNum[entityType] = num;
        }
    }

    public void ClearAll()
    {
        _killEntityNum.Clear();
    }
}