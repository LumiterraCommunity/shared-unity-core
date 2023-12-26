/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本管理
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/InstancingMgrCore.cs
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
    public int CurLevelIndex = 0; //当前关卡
    public long CurLevelStartTime = 0; //当前关卡开始时间
    public long InstancingStartTime = 0; //副本开始时间
    public static GameObject Root { get; private set; }
    private void Awake()
    {
        if (Root == null)
        {
            Root = new GameObject("InstancingMgr");
            Root.transform.SetParent(gameObject.transform);
        }
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
    public virtual bool CompleteLevel(int index, bool isSuccess)
    {
        if (index < 0 || index >= LevelList.Count)
        {
            Log.Error($"InstancingMgr CompleteLevel Error: index = {index}");
            return false;
        }

        if (!LevelList[index].CompleteLevel(isSuccess))
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

}
