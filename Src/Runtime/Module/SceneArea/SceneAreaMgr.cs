/*
 * @Author: xiang huan
 * @Date: 2022-12-02 10:52:02
 * @Description: 区域管理
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneArea/SceneAreaMgr.cs
 * 
 */


using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
//区域名字
public class SceneAreaMgr : SceneModuleBase
{
    /// <summary>
    /// 家进入新的场景的检查区域事件
    /// <param long>玩家实体ID</param>
    /// <param eSceneArea>新区域标识</param>
    /// </summary>
    public Action<long, eSceneArea> OnPlayerEnterNewSceneCheckArea = delegate { };
    /// <summary>
    /// 玩家退出当前场景的检查区域事件
    /// <param long>玩家实体ID</param>
    /// <param eSceneArea>新区域标识</param>
    /// </summary>
    public Action<long, eSceneArea> OnPlayerExitCurSceneCheckArea = delegate { };
    /// <summary>
    /// 当前区域改变事件
    /// </summary>
    public Action<eSceneArea> OnCurAreaChanged = delegate { };
    /// <summary>
    /// 默认区域改变事件
    /// </summary>
    public Action<eSceneArea> OnDefaultAreaChanged = delegate { };
    /// <summary>
    /// 当前区域，对于前端来说是当前连接的服务器地图，对于后端来说是当前的服务器地图
    /// </summary>
    /// <value></value>
    public eSceneArea CurArea { get; private set; } = eSceneArea.None;
    /// <summary>
    /// 默认区域，对于前端来说，就是当前的主场景，对于后端来说，就是当前的服务器地图
    /// eg:当在家园时，CurArea为家园，DefaultArea为大世界，因为家园是无缝切换的，进入家园的时候，主场景还是大世界
    /// </summary>
    /// <value></value>
    public eSceneArea DefaultArea { get; private set; } = eSceneArea.None;
    /// <summary>
    /// 当前区域对应的配置
    /// </summary>
    /// <value></value>
    public DRSceneArea CurDRSceneArea { get; private set; }
    /// <summary>
    /// 默认区域对应的配置
    /// </summary>
    /// <value></value>
    public DRSceneArea DefaultDRSceneArea { get; private set; }

    // private readonly List<eSceneArea> _areaQueue = new();
    /// <summary>
    /// 设置默认区域
    /// </summary>
    public void SetDefaultArea(eSceneArea area)
    {
        SetCurArea(area);//进入默认区域时，当前区域也要改变
        if (DefaultArea == area)
        {
            return;
        }

        DefaultArea = area;
        DefaultDRSceneArea = GetSceneAreaCfg(DefaultArea);
        OnDefaultAreaChanged.Invoke(DefaultArea);
    }

    public void SetCurArea(eSceneArea area)
    {
        if (CurArea == area)
        {
            return;
        }

        CurArea = area;
        CurDRSceneArea = GetSceneAreaCfg(CurArea);
        OnCurAreaChanged.Invoke(CurArea);
    }

    private DRSceneArea GetSceneAreaCfg(eSceneArea area)
    {
        if (area == eSceneArea.None)
        {
            return null;
        }

        return GFEntryCore.DataTable.GetDataTable<DRSceneArea>().GetDataRow((int)area);
    }

    public bool IsDefaultAreaType(eSceneType sceneType)
    {
        return DefaultDRSceneArea != null && DefaultDRSceneArea.SceneType == (int)sceneType;
    }

    /// <summary>
    /// 是否为副本
    /// </summary>
    public bool IsInstancing()
    {
        return CurDRSceneArea != null && CurDRSceneArea.SceneType == (int)eSceneType.Instancing;
    }

    public eSceneType GetCurAreaType()
    {
        if (CurDRSceneArea == null)
        {
            return eSceneType.Unknown;
        }

        return (eSceneType)CurDRSceneArea.SceneType;
    }
    /// <summary>
    /// 进入区域
    /// </summary>
    // public void EnterArea(eSceneArea area)
    // {
    //     _areaQueue.Add(area);
    // }

    // /// <summary>
    // /// 离开区域
    // /// </summary>
    // public void ExitArea(eSceneArea area)
    // {
    //     _ = _areaQueue.Remove(area);
    // }

    /// <summary>
    /// 获取当前区域
    /// </summary>
    // public eSceneArea GetCurArea()
    // {
    //     if (_areaQueue.Count > 0)
    //     {
    //         return _areaQueue[^1];
    //     }
    //     return DefaultArea;
    // }
}