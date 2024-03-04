using System;
using UnityEngine;

/// <summary>
/// 实体检测雷达辅助类 放在碰撞盒上一起检测用的 实体雷达则是放到实体本体上的
/// </summary>
public class RadarHelper : MonoBehaviour
{
    public event Action<Collider> OnOneTriggerEnter;
    public event Action<Collider> OnOneTriggerExit;

    private void OnDestroy()
    {
        OnOneTriggerEnter = null;
        OnOneTriggerExit = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnOneTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnOneTriggerExit?.Invoke(other);
    }
}