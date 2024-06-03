using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 家园土地管理接口 往往给共享库内部获取土地信息用
/// </summary>
public interface IHomeSoilMgr
{
    /// <summary>
    /// 添加创建一块土地
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos">世界坐标 土地中心点</param>
    public void AddSoil(ulong id, Vector3 pos);

    /// <summary>
    /// 删除销毁一个土地
    /// </summary>
    /// <param name="id"></param>
    public void RemoveSoil(ulong id);

    /// <summary>
    /// 获取某个土地 不存在返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public HomeSoilCore GetSoil(ulong id);
    /// <summary>
    /// 获取所有的土地 不要修改内部结构 没有分配GC
    /// </summary>
    /// <returns></returns>
    IEnumerable<HomeSoilCore> GetAllSoil();
}