using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 水伤害检测
/// </summary>
public class WaterDeathDetectionCore : SceneDamageTriggerBaseCore
{
    public override GameMessageCore.DamageState DamageState => GameMessageCore.DamageState.WaterDrown;
    private readonly HashSet<Collider> _curTriggerWater = new();
    private RoleBaseDataCore _roleBaseDataCore;

    private void Start()
    {
        _ = TryGetComponent(out _roleBaseDataCore);
    }

    private void OnDestroy()
    {
        _curTriggerWater.Clear();
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
        if (_curTriggerWater.Count == 0)
        {
            return;
        }

        if (CheckTrigger())
        {
            TriggerSceneDamage();
        }
    }

    protected override bool CheckTrigger()
    {
        if (!base.CheckTrigger())
        {
            return false;
        }
        // 检测是否在水中
        Vector3 headPos = _roleBaseDataCore != null ? _roleBaseDataCore.TopPos : transform.position;
        return Physics.CheckSphere(headPos, 0.1f, 1 << MLayerMask.WATER, QueryTriggerInteraction.Collide);
    }
}