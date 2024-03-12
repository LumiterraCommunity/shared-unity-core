using UnityEngine;

/// <summary>
/// 家园实体组件基类 方便拿一些东西
/// </summary>
/// <typeparam name="THomeEntity"></typeparam>
public class HomeEntityComponentCore<THomeEntity> : MonoBehaviour where THomeEntity : HomeEntityCore
{
    /// <summary>
    /// 挂载对应的家园实体
    /// </summary>
    /// <value></value>
    public THomeEntity RefEntity { get; private set; }

    /// <summary>
    /// 对应的土地
    /// </summary>
    public HomeSoilCore RefSoil => RefEntity.RefSoil;

    protected virtual void Awake()
    {
        RefEntity = GetComponent<THomeEntity>();
    }
}