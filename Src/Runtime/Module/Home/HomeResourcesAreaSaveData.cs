using System.Collections.Generic;
using System;

/// <summary>
/// 单个资源区域 可能是数据库的  也可能是服务器转给客户端的
/// </summary>
[Serializable]
public class HomeResourcesAreaSaveData
{
    /// <summary>
    /// 区域唯一ID
    /// </summary>
    public int Id;
    /// <summary>
    /// 下次刷新时间
    /// </summary>
    public long UpdateTime;
    /// <summary>
    /// 资源列表
    /// </summary>
    public List<HomeResourcesPointSaveData> PointList;
}