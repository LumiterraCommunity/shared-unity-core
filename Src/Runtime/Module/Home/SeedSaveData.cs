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
    /// 当前种子的nftId,比如图腾等需要明确当时种植的nftId
    /// </summary>
    public string SeedNftId = string.Empty;
    /// <summary>
    /// 施的肥料配置ID
    /// </summary>
    public int ManureCid;
    /// <summary>
    /// 当前剩余额外浇水次数
    /// </summary>
    public int ExtraWateringNum;
    /// <summary>
    /// 当前积累的熟练度值 不要在这里直接设置 这里只读
    /// </summary>
    public int CurProficiency;
    /// <summary>
    /// 是否需要腐败收获 不要在这里直接设置 这里只读
    /// </summary>
    public bool NeedPerish;
    /// <summary>
    /// 种子实体id 一般是0 只有特殊种子 还是播种就由数据服分配了id的才有值
    /// </summary>
    public long SeedEntityId;
    /// <summary>
    /// 种子等级的整数位等级 配置表单位整数位
    /// </summary>
    public int Lv;
    /// <summary>
    /// 种子等级的小数位扩展等级 配置表单位整数位千分位
    /// </summary>
    public int ExtThousLv;
    /// <summary>
    /// 当前潜力值 配置表单位整数位千分位
    /// </summary>
    public int Potentiality;

    /// <summary>
    /// 是否有种子
    /// </summary>
    /// <returns></returns>
    public bool HaveSeed()
    {
        return SeedCid > 0;
    }

    public SeedSaveData()
    {
    }

    public SeedSaveData(ProxySeedData data)
    {
        GrowingStage = data.GrowingStage;
        SeedCid = data.SeedCid;
        ManureCid = data.ManureCid;
        ExtraWateringNum = data.ExtraWateringNum;
        CurProficiency = data.CurProficiency;
        NeedPerish = data.NeedPerish;
        SeedNftId = data.SeedNftId;
        SeedEntityId = data.SeedEntityId;
        Lv = data.Lv;
        ExtThousLv = data.ExtThousLv;
        Potentiality = data.Potentiality;
    }

    internal ProxySeedData ToProxySeedData()
    {
        return new ProxySeedData()
        {
            GrowingStage = GrowingStage,
            SeedCid = SeedCid,
            ManureCid = ManureCid,
            ExtraWateringNum = ExtraWateringNum,
            CurProficiency = CurProficiency,
            NeedPerish = NeedPerish,
            SeedNftId = SeedNftId ?? string.Empty,
            SeedEntityId = SeedEntityId,
            Lv = Lv,
            ExtThousLv = ExtThousLv,
            Potentiality = Potentiality
        };
    }
}