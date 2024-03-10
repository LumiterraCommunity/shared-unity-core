using UnityEngine;

/// <summary>
/// 家园实体基类core
/// </summary>
public class HomeEntityCore : MonoBehaviour
{
    /// <summary>
    /// 关联引用的土地
    /// </summary>
    /// <value></value>
    public HomeSoilCore RefSoil { get; private set; }

    /// <summary>
    /// 初始化工作
    /// </summary>
    /// <param name="soil"></param>
    internal virtual void Init(HomeSoilCore soil)
    {
        RefSoil = soil;
    }
}