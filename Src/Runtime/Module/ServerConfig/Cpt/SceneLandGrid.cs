
using UnityEngine;

/// <summary>
/// 场景领地格子（编辑器铺格子用）
/// </summary>
public class SceneLandGrid : MonoBehaviour
{
    public GameObject Frame;

    public void ShowFrame(bool active)
    {
        Frame.SetActive(active);
    }
}