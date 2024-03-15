using GameMessageCore;

/// <summary>
/// 图腾上的专有数据
/// </summary>
public class TotemDataCore : SeedEntityComponentCore<SeedEntityCore>
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
    /// 从网络数据初始化 只有初始化后才是有效实体
    /// </summary>
    /// <param name="proxyData"></param>
    internal void InitFromProxyData(ProxyTotemData proxyData)
    {
        Inited = true;

        InvestDungeon = proxyData.InvestDungeon;
        PrizeLp = proxyData.PrizeLp;
        RewardLp = proxyData.RewardLp;
    }
}