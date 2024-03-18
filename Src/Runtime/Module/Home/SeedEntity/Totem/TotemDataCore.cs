using System;
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 图腾上的专有数据
/// </summary>
public class TotemDataCore : SeedEntityComponentCore<SeedEntityCore>, ISeedEntitySpecialData
{
    /// <summary>
    /// 没有初始化的实体是无效的
    /// </summary>
    /// <value></value>
    public bool Inited { get; private set; }

    /// <summary>
    /// 投资的图腾副本 未投资=0
    /// </summary>
    public int InvestDungeon;
    /// <summary>
    /// 投资份额
    /// </summary>
    public string PrizeLp;
    /// <summary>
    /// 回报份额
    /// </summary>
    public string RewardLp;

    /// <summary>
    /// 有投资份额
    /// </summary>
    /// <returns></returns>
    public bool IsHavePrizeLp()
    {
        return !string.IsNullOrEmpty(PrizeLp) && PrizeLp != "0";//后端约定0或者空表示没有份额
    }

    /// <summary>
    /// 有可以收获的收益
    /// </summary>
    /// <returns></returns>
    public bool IsHaveReward()
    {
        return !string.IsNullOrEmpty(RewardLp) && RewardLp != "0";//后端约定0或者空表示没有份额
    }

    /// <summary>
    /// 从网络数据初始化 只有初始化后才是有效实体
    /// </summary>
    /// <param name="proxyData"></param>
    internal void InitFromProxyData(ProxyTotemData proxyData)
    {
        Inited = true;

        SetProxyData(proxyData);
    }

    /// <summary>
    /// 初始化一个新实体
    /// </summary>
    internal void InitFromNewEntity()
    {
        Inited = true;

        InvestDungeon = 0;
        PrizeLp = string.Empty;
        RewardLp = string.Empty;
    }

    public void SetProxyData(ProxyTotemData proxyData)
    {
        if (proxyData == null)
        {
            Log.Error($"UpdateData proxyData is null,id:{RefEntity.Id}");
            return;
        }

        if (!Inited)
        {
            Log.Error($"UpdateData not inited,id:{RefEntity.Id}");
            return;
        }

        InvestDungeon = proxyData.InvestDungeon;
        PrizeLp = proxyData.PrizeLp;
        RewardLp = proxyData.RewardLp;

        RefEntity.EntityEvent.OnTotemDataUpdated?.Invoke();
    }

    public void FillProxyData(ProxySeedEntityData proxyData)
    {
        if (!Inited)
        {
            Log.Error($"FillProxyData not inited,id:{RefEntity.Id}");
            return;
        }

        proxyData.TotemData = new ProxyTotemData()
        {
            InvestDungeon = InvestDungeon,
            PrizeLp = PrizeLp,
            RewardLp = RewardLp,
        };
    }
}