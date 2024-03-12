using UnityEngine;

/// <summary>
/// 种子实体组件基类 方便拿一些东西
/// </summary>
/// <typeparam name="TSeedEntity"></typeparam>
public class SeedEntityComponentCore<TSeedEntity> : MonoBehaviour where TSeedEntity : SeedEntityCore
{
    /// <summary>
    /// 挂载对应的种子实体
    /// </summary>
    /// <value></value>
    public TSeedEntity RefEntity { get; private set; }

    /// <summary>
    /// 对应的土地
    /// </summary>
    public HomeSoilCore RefSoil => RefEntity.RefSoil;

    protected virtual void Awake()
    {
        RefEntity = GetComponent<TSeedEntity>();
    }
}