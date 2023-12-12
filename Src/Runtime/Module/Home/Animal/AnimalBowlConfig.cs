using UnityEngine;

/// <summary>
/// 畜牧食盆的场景配置
/// </summary>
public class AnimalBowlConfig : SharedCoreComponent
{
    [Header("食盆的id 需要唯一 也是吃的顺序")]
    public ulong BowlId;
}