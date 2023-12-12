
using UnityEngine;

/// <summary>
/// 仅用于策划配置场景中的物体占领地格子数
/// </summary>
public class SceneLandGridCfg : MonoBehaviour
{
    [Header("X方向占格子数")]
    public int LandCountX = 1;
    [Header("Z方向占格子数")]
    public int LandCountZ = 1;
}