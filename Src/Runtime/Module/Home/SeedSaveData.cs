using System;
using GameMessageCore;

/// <summary>
/// 种子相关的保存数据
/// </summary>
[Serializable]
public class SeedSaveData
{
    /// <summary>
    /// 当前生成的阶段 从0开始 只对生长阶段有效 不要在这里直接设置 需要走SoilData.SetGrowStage
    /// </summary>
    public int GrowingStage = -1;
    /// <summary>
    /// 当前种子配置ID 只有放了种子的状态才有效 不要在这里直接设置 需要走SoilData.SetSeedCid
    /// </summary>
    public int SeedCid;
    /// <summary>
    /// 施的肥料配置ID
    /// </summary>
    public int ManureCid;
    /// <summary>
    /// 当前剩余额外浇水次数
    /// </summary>
    public int ExtraWateringNum;

    public SeedSaveData()
    {
    }

    public SeedSaveData(ProxySeedData data)
    {
        GrowingStage = data.GrowingStage;
        SeedCid = data.SeedCid;
        ManureCid = data.ManureCid;
        ExtraWateringNum = data.ExtraWateringNum;
    }

    internal ProxySeedData ToProxySeedData()
    {
        return new ProxySeedData()
        {
            GrowingStage = GrowingStage,
            SeedCid = SeedCid,
            ManureCid = ManureCid,
            ExtraWateringNum = ExtraWateringNum,
        };
    }
}