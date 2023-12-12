using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 水域淹死检测
/// </summary>
public class WaterDeathDetectionCore : EntityBaseComponent, ISceneDamageDetection
{
    private readonly HashSet<Collider> _curTriggerWater = new();
    private bool _isDetecting = false;
    private RoleBaseDataCore _roleBaseDataCore;

    private void Start()
    {
        StartDetection();

        _ = TryGetComponent(out _roleBaseDataCore);
    }

    private void OnDestroy()
    {
        _curTriggerWater.Clear();
    }

    public void StartDetection()
    {
        _isDetecting = true;
    }

    public void StopDetection()
    {
        _isDetecting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.WATER)
        {
            return;
        }

        if (!_curTriggerWater.Add(other))
        {
            Log.Error($"WaterDeathDetectionCore enter water repeated, name={other.gameObject.name}");
        }

        if (_curTriggerWater.Count == 1)
        {
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.WATER)
        {
            return;
        }

        if (!_curTriggerWater.Remove(other))
        {
            Log.Error($"WaterDeathDetectionCore exit water not exist, name={other.gameObject.name}");
        }

        if (_curTriggerWater.Count == 0)
        {
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_isDetecting || _curTriggerWater.Count == 0)
        {
            return;
        }

        if (CheckHeadUnderWater())
        {
            StopDetection();

            OnWaterDeath();
        }
    }

    private bool CheckHeadUnderWater()
    {
        Vector3 headPos = _roleBaseDataCore != null ? _roleBaseDataCore.TopPos : transform.position;
        return Physics.CheckSphere(headPos, 0.1f, 1 << MLayerMask.WATER, QueryTriggerInteraction.Collide);
    }

    private void OnWaterDeath()
    {
        RefEntity.EntityEvent.OnSceneDeath?.Invoke(GameMessageCore.DamageState.WaterDrown);
    }
}