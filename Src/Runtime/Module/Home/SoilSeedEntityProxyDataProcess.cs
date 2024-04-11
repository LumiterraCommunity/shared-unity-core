using GameMessageCore;
using UnityEngine;

/// <summary>
/// 挂载土地上的种子实体的数据处理地方 用来存储网络传输的初始数据和加工本地数据到网络传输数据
/// </summary>
public class SoilSeedEntityProxyDataProcess : MonoBehaviour
{
    /// <summary>
    /// 初始化的传输数据 新种的时候为null 如果只是没成熟没长出实体则是里面的特殊数据为空 这里并不会为空
    /// </summary>
    /// <value></value>
    public ProxySeedEntityData InitProxyData { get; private set; }

    /// <summary>
    /// 设置初始化的传输数据 给实体生成时来处理自己的特殊数据
    /// </summary>
    internal void SetInitProxyData(ProxySeedEntityData data)
    {
        InitProxyData = data;
    }
}