/*
 * @Author: xiang huan
 * @Date: 2024-08-08 13:54:21
 * @Description: 场景伤害触发检测
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/SceneDamage/SceneDamageTriggerBaseCore.cs
 * 
 */
using GameMessageCore;
using UnityEngine;

/// <summary>
/// 水域淹死检测
/// </summary>
public class SceneDamageTriggerBaseCore : EntityBaseComponent, ISceneDamageDetection
{
    public virtual float SceneDamageTriggerInterval => 1f; //场景伤害间隔时间
    public virtual DamageState DamageState => DamageState.Fall;
    private float _triggerTime = 0;
    protected virtual void TriggerSceneDamage()
    {
        if (Time.realtimeSinceStartup - _triggerTime < SceneDamageTriggerInterval)
        {
            return;
        }
        _triggerTime = Time.realtimeSinceStartup;
        RefEntity.EntityEvent.OnSceneDamage?.Invoke(DamageState);
    }

    protected virtual bool CheckTrigger()
    {
        //宿主不存在 或者 宿主已经死亡
        if (RefEntity == null || RefEntity.BattleDataCore == null || !RefEntity.BattleDataCore.IsLive())
        {
            return false;
        }
        return true;
    }
}