/*
 * @Author: xiang huan
 * @Date: 2022-12-06 10:27:50
 * @Description: 资源刷新区域
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Home/HomeResourcesArea.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityGameFramework.Runtime;
public class HomeResourcesArea : SharedCoreComponent
{
    [Header("唯一区域ID(不可重复)")]
    public int Id;

    [Header("是否绘制形状")]
    public bool IsDraw;

    // [Header("区域类型")]
    // public HomeDefine.eHomeResourcesAreaType AreaType;

    // [Header("刷新间隔(ms)")]
    // public int UpdateInterval;

    // [Header("立即刷新")]
    // public bool IsUpdateNow = false;

    // [Serializable]
    // public struct HomeResourcesAreaPoint
    // {
    //     [Header("权重(百分位)")]
    //     public int Weight;
    //     [Header("配置ID")]
    //     public int ConfigId;
    //     [Header("刷新上限")]
    //     public int LimitNum;
    // }
    // [Header("资源列表")]
    // public List<HomeResourcesAreaPoint> PointList;

    public Bounds AreaBounds { get; private set; }
    public HomeResourcesAreaSaveData SaveData { get; private set; }  //保存数据

    public DRHomeResourceArea DRHomeResourceArea { get; private set; }
    private void Awake()
    {
        AreaBounds = new Bounds(transform.position, transform.localScale);
        DRHomeResourceArea = GFEntryCore.DataTable.GetDataTable<DRHomeResourceArea>().GetDataRow(Id);
        if (DRHomeResourceArea == null)
        {
            Log.Error("HomeResourcesArea Awake Error: Can't find config id = " + Id);
            return;
        }
        SaveData = CreateSaveData();
        GFEntryCore.HomeResourcesAreaMgr.AddArea(this);
    }


    private void OnDestroy()
    {
        GFEntryCore.HomeResourcesAreaMgr.RemoveArea(Id);
    }

    /// <summary>
    /// 创建存储数据
    /// </summary>
    protected HomeResourcesAreaSaveData CreateSaveData()
    {
        if (DRHomeResourceArea == null)
        {
            return null;
        }
        long updateTime = (GFEntryCore.GFEntryType == GFEntryType.Client ? TimeUtil.GetTimeStamp() : TimeUtil.GetServerTimeStamp()) + DRHomeResourceArea.UpdateInterval;
        if (DRHomeResourceArea.IsUpdateNow)
        {
            updateTime = 0;
        }
        HomeResourcesAreaSaveData data = new()
        {
            Id = Id,
            PointList = new(),
            //这里客户端使用自己的本地时间 因为这些是在加载场景时就加载就初始化了 没有同步服务器时间 而且看现在逻辑 也不用这个时间 只是和服务器共享了逻辑而已
            UpdateTime = updateTime
        };
        return data;
    }

    /// <summary>
    /// 初始化存储数据
    /// </summary>
    public void SetSaveData(HomeResourcesAreaSaveData saveData)
    {
        if (saveData == null)
        {
            return;
        }
        SaveData = saveData;
    }

    /// <summary>
    /// 获得存储数据
    /// </summary>
    public HomeResourcesAreaSaveData GetSaveData()
    {
        return SaveData;
    }

    /// <summary>
    /// 设置存储数据刷新时间
    /// </summary>
    public void SetSaveDataUpdateTime(long time)
    {
        if (SaveData == null)
        {
            return;
        }
        SaveData.UpdateTime = time;
    }

    /// <summary>
    /// 添加存储数据资源点
    /// </summary>
    public void AddSaveDataPoint(HomeResourcesPointSaveData pointData)
    {
        if (SaveData == null)
        {
            return;
        }
        SaveData.PointList.Add(pointData);
    }

    /// <summary>
    /// 删除存储数据资源点
    /// </summary>
    public void RemoveSaveDataPoint(ulong id)
    {
        if (SaveData == null)
        {
            return;
        }
        for (int i = 0; i < SaveData.PointList.Count; i++)
        {
            if (SaveData.PointList[i].Id == id)
            {
                SaveData.PointList.RemoveAt(i);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!IsDraw)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMatrix;
    }
}