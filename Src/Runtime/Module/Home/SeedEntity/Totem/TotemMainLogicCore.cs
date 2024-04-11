using System;
using GameMessageCore;

/// <summary>
/// 图腾主逻辑
/// </summary>
public class TotemMainLogicCore : SeedEntityComponentCore<SeedEntityCore>
{
    protected virtual void Start()
    {
        SoilSeedEntityProxyDataProcess dataProcess = RefSoil.GetComponent<SoilSeedEntityProxyDataProcess>();

        if (dataProcess.InitProxyData?.TotemData != null)//初始化就有实体
        {
            InitEntityData(dataProcess.InitProxyData.TotemData);
        }
        else//新成熟生成的实体
        {
            GetComponent<TotemDataCore>().InitFromNewEntity();//军杰说直接用默认数据 没有给默认数据接口
            OnGenerateNewEntityWhenRipe();
        }
    }

    public void InitEntityData(ProxyTotemData proxyData)
    {
        GetComponent<TotemDataCore>().InitFromProxyData(proxyData);
    }

    /// <summary>
    /// 成熟时生成一个新实体时调用
    /// </summary>
    protected virtual void OnGenerateNewEntityWhenRipe()
    {
    }
}