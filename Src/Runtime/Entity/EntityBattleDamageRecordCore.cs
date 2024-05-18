/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体伤害记录
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityBattleDamageRecordCore.cs
 * 
 */
using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体伤害记录
/// </summary>
public class EntityBattleDamageRecordCore : EntityBaseComponent
{
    public int TotalDamage = 0; //总伤害
    private void Start()
    {
        RefEntity.EntityEvent.EntityBattleAddDamageRecord += EntityBattleAddDamageRecord;
    }

    private void OnDestroy()
    {
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.EntityBattleAddDamageRecord -= EntityBattleAddDamageRecord;
        }
    }

    private void EntityBattleAddDamageRecord(int damageValue)
    {
        TotalDamage += damageValue;
        RefEntity.EntityEvent.EntityBattleDamageRecordChange?.Invoke();
    }

    public void StartRecord(int damageValue)
    {
        TotalDamage = damageValue;
    }
}