using System;
using GameMessageCore;
using static HomeDefine;

/// <summary>
/// 单个土地的存储数据 可能是数据库的  也可能是服务器转给客户端的
/// </summary>
[Serializable]
public class SoilSaveData
{
    /// <summary>
    /// 土地唯一ID
    /// </summary>
    public ulong Id;

    /// <summary>
    /// 土地状态
    /// </summary>
    public eSoilStatus SoilStatus;
    /// <summary>
    /// 开始当前状态的时间
    /// </summary>
    public long StatusStartStamp;

    /// <summary>
    /// 土地肥沃度 0代表无效
    /// </summary>
    public int Fertile;

    /// <summary>
    /// 种子相关保存数据
    /// </summary>
    public SeedSaveData SeedData { get; private set; } = new();

    public SoilSaveData()
    {
    }

    public void ClearSeedData()
    {
        SeedData = new SeedSaveData();
    }

    public SoilSaveData(ProxySoilData data)
    {
        Id = data.Id;
        SoilStatus = (eSoilStatus)data.SoilStatus;
        StatusStartStamp = data.StatusStartStamp;
        Fertile = data.Fertile;
        SeedData = new SeedSaveData(data.SeedData);
    }

    public ProxySoilData ToProxySoilData()
    {
        return new ProxySoilData()
        {
            Id = Id,
            SoilStatus = (int)SoilStatus,
            StatusStartStamp = StatusStartStamp,
            Fertile = Fertile,
            SeedData = SeedData.ToProxySeedData(),
        };
    }
}