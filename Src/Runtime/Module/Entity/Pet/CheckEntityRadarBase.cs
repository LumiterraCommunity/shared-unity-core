using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检测周围实体的雷达基类 利用碰撞盒检测周围实体
/// </summary>
public abstract class CheckEntityRadarBase : MonoBehaviour
{
    protected abstract float radius { get; }
    protected abstract LayerMask layer { get; }

    // public long GetNearestEntity()
    // {
    //TODO: pet
    // }
}