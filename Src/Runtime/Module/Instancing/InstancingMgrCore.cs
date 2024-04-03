/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本管理
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/InstancingMgrCore.cs
 * 
 */
using System;
using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class InstancingMgrCore<TLevel> : MonoBehaviour, IInstancingMgr where TLevel : InstancingLevelBase
{
    public bool IsInit { get; set; } = false; //是否初始化
    public eInstancingStatusType StatusType = eInstancingStatusType.InstancingInactive;       //副本状态
    public List<TLevel> LevelList = new(); //关卡列表
    public ListMap<long, PlayerInstancingData> PlayerInstancingData = new(); //玩家副本数据
    public int CurLevelIndex = 0; //当前关卡
    public long CurLevelStartTime { get; set; } = 0; //当前关卡开始时间
    public long InstancingStartTime = 0; //副本开始时间
    public bool IsMatchComplete = false; //是否匹配完成
    public InstancingTotemData TotemData = new(); //副本图腾数据
    public static GameObject Root { get; private set; }
    private void Awake()
    {
        if (Root == null)
        {
            Root = new GameObject("InstancingMgr");
            Root.transform.SetParent(gameObject.transform);
        }
    }

    public virtual void CompleteMatch()
    {
        IsMatchComplete = true;
    }

    /// <summary>
    /// 设置当前关卡
    /// </summary>
    /// <param name="index"></param>
    public virtual bool SetCurLevel(int index)
    {
        if (index < 0 || index >= LevelList.Count)
        {
            Log.Error($"InstancingMgr StartLevel Error: index = {index}");
            return false;
        }

        if (!LevelList[index].RunLevel())
        {
            return false;
        }
        CurLevelIndex = index;
        CurLevelStartTime = TimeUtil.GetServerTimeStamp();
        LevelStatusChange(index);
        MessageCore.LevelStatusUpdate?.Invoke(index, LevelList[index].StatusType);
        return true;
    }
    /// <summary>
    /// 完成关卡
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSuccess"></param>
    /// <param name="isReward"></param>
    public virtual bool CompleteLevel(int index, bool isSuccess, bool isReward = true)
    {
        if (index < 0 || index >= LevelList.Count)
        {
            Log.Error($"InstancingMgr CompleteLevel Error: index = {index}");
            return false;
        }

        if (!LevelList[index].CompleteLevel(isSuccess, isReward))
        {
            return false;
        }
        LevelStatusChange(index);
        MessageCore.LevelStatusUpdate?.Invoke(index, LevelList[index].StatusType);
        return true;
    }

    /// <summary>
    /// 重置关卡
    /// </summary>
    /// <param name="index"></param>
    public virtual bool ResetLevel(int index)
    {
        if (index < 0 || index >= LevelList.Count)
        {
            Log.Error($"InstancingMgr ResetLevel Error: index = {index}");
            return false;
        }
        if (!LevelList[index].ResetLevel())
        {
            return false;
        }
        LevelStatusChange(index);
        MessageCore.LevelStatusUpdate?.Invoke(index, LevelList[index].StatusType);
        return true;
    }

    /// <summary>
    /// 完成副本
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="isAll"></param>
    public virtual void CompleteInstancing(bool isSuccess, bool isAll = false)
    {
        StatusType = isSuccess ? eInstancingStatusType.InstancingSuccess : eInstancingStatusType.InstancingFailure;
    }

    public eInstancingStatusType GetLevelStatus(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= LevelList.Count)
        {
            Log.Error($"InstancingMgr GetLevelStatus Error: levelIndex = {levelIndex}");
            return eInstancingStatusType.InstancingInactive;
        }
        return LevelList[levelIndex].StatusType;
    }

    public TLevel GetLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= LevelList.Count)
        {
            Log.Error($"InstancingMgr GetLevel Error: levelIndex = {levelIndex}");
            return null;
        }
        return LevelList[levelIndex];
    }

    protected virtual void LevelStatusChange(int index)
    {

    }

    public virtual PlayerInstancingData GetOrAddPlayerData(long playerId)
    {
        if (PlayerInstancingData.TryGetValueFromKey(playerId, out PlayerInstancingData playerData))
        {
            return playerData;
        }

        playerData = new();
        playerData.SetData(playerId);
        _ = PlayerInstancingData.Add(playerId, playerData);
        return playerData;
    }


    public PlayerInstancingData GetPlayerData(long playerId)
    {
        if (PlayerInstancingData.TryGetValueFromKey(playerId, out PlayerInstancingData playerData))
        {
            return playerData;
        }
        return null;
    }
    public float ScoreToRewards(float score)
    {
        if (TotemData.TotalScore <= 0)
        {
            return 0;
        }
        return score * TotemData.BaseRewards / TotemData.TotalScore;
    }
    public float RandomRewardsRate(int areaLevel)
    {
        if (TotemData.BaseRewards <= 0)
        {
            return 0;
        }
        float curRate = TotemData.TotalRewards / TotemData.BaseRewards;
        float mimRateRange = 0.66f;
        float maxRateRange = 0.83f;
        if (TableUtil.TryGetGameValue(eGameValueID.InstancingRewardsRateRange, out DRGameValue drGameValue))
        {
            mimRateRange = drGameValue.ValueArray[0] * TableDefine.THOUSANDTH_2_FLOAT;
            maxRateRange = drGameValue.ValueArray[1] * TableDefine.THOUSANDTH_2_FLOAT;
        }
        //根据区域等级调整奖励倍率, 3级区域不调整, 2级区域调整66%-83%, 1级区域调整66%-83%
        int level = Math.Max(0, InstancingDefine.MAX_AREA_LEVEL - areaLevel);
        for (int i = 0; i < level; i++)
        {
            float rate = UnityEngine.Random.Range(mimRateRange, maxRateRange);
            curRate *= rate;
        }
        return curRate;
    }
}
