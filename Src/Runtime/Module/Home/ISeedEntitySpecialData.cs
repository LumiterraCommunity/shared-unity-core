using GameMessageCore;

/// <summary>
/// 种子实体上的特殊数据接口
/// </summary>
internal interface ISeedEntitySpecialData
{
    /// <summary>
    /// 给上层的种子实体数据填充自己特殊传输数据
    /// </summary>
    /// <param name="proxyData"></param>
    void FillProxyData(ProxySeedEntityData proxyData);
}