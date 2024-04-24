/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 战斗数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityBattleDataCore.cs
 * 
 */
using System;
using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体上的战斗数据 双端通用的核心数据
/// </summary>
public class EntityBattleDataCore : EntityBaseComponent
{
    /// <summary>
    /// 当前血量
    /// </summary>
    /// <value></value>
    public int HP { get => GetHpValue(); protected set => SetHpValue(value); }
    private IntAttribute _hpAttribute;  //血量属性。会高频调用，单独存储优化性能
    /// <summary>
    /// 最大血量
    /// </summary>
    /// <value></value>
    public int HPMAX { get => GetValue(eAttributeType.MaxHP); protected set => SetBaseValue(eAttributeType.MaxHP, value); }
    /// <summary>
    /// 血量回复
    /// </summary>
    /// <value></value>
    public int HPRecovery { get => GetValue(eAttributeType.HpRecovery); protected set => SetBaseValue(eAttributeType.HpRecovery, value); }
    /// <summary>
    /// 攻击力
    /// </summary>
    public int Att { get => GetValue(eAttributeType.CombatAtt); protected set => SetBaseValue(eAttributeType.CombatAtt, value); }
    /// <summary>
    /// 防御力
    /// </summary>
    public int Def { get => GetValue(eAttributeType.CombatDef); protected set => SetBaseValue(eAttributeType.CombatDef, value); }
    /// <summary>
    /// 普通攻击速度
    /// </summary>
    public int AttSpeed { get => GetValue(eAttributeType.CombatAttSpd); protected set => SetBaseValue(eAttributeType.CombatAttSpd, value); }
    /// <summary>
    /// 暴击率
    /// </summary>
    public int CritRate { get => GetValue(eAttributeType.CombatCritRate); protected set => SetBaseValue(eAttributeType.CombatCritRate, value); }
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public int CritDmg { get => GetValue(eAttributeType.CombatCritDmg); protected set => SetBaseValue(eAttributeType.CombatCritDmg, value); }
    /// <summary>
    /// 命中率
    /// </summary>
    public int HitRate { get => GetValue(eAttributeType.CombatHit); protected set => SetBaseValue(eAttributeType.CombatHit, value); }
    /// <summary>
    /// miss率
    /// </summary>
    public int MissRate { get => GetValue(eAttributeType.CombatDodge); protected set => SetBaseValue(eAttributeType.CombatDodge, value); }
    /// <summary>
    /// 移动速度 属性是cm转成m
    /// </summary>
    public float MoveSpeed { get => GetMoveSpeedValue(); protected set => SetBaseValue(eAttributeType.MoveSpd, (int)(value * MathUtilCore.M2CM)); }
    /// <summary>
    /// 等级,这里取的是战斗专精等级
    /// </summary>
    public int Level { get => GetValue(eAttributeType.CombatLv); protected set => SetBaseValue(eAttributeType.CombatLv, value); }
    /// <summary>
    /// 经验
    /// </summary>
    public long Exp { get; protected set; }
    /// <summary>
    /// 是否在战斗状态中
    /// </summary>
    /// <value></value>
    public bool IsInBattle { get; protected set; }

    /// <summary>
    /// 是否进入灵魂状态
    /// </summary>
    /// <value></value>
    public bool IsInSoul => Status == EntityStatus.Soul;
    /// <summary>
    /// 是否进入死亡状态
    /// </summary>
    /// <value></value>
    public bool IsInDead => Status == EntityStatus.Dead;

    /// <summary>
    /// 实体状态
    /// </summary>
    public EntityStatus Status;

    /// <summary>
    /// 死亡原因 只在hp<=0时有效
    /// </summary>
    /// <value></value>
    public DamageState DeathReason { get; private set; }
    /// <summary>
    /// 战斗状态map  <状态key，添加计数>
    /// </summary>
    private readonly Dictionary<BattleDefine.eBattleState, int> _battleStateMap = new();

    private PlayerAreaRecord _playerAreaRecord;

    public int RandomSeed { get; private set; }
    private void Start()
    {
        _playerAreaRecord = GetComponent<PlayerAreaRecord>();
    }
    public virtual void SetHP(int hp, bool isForce = false)
    {
        HP = System.Math.Clamp(hp, 0, HPMAX);
    }

    /// <summary>
    /// 设置死亡原因 在死亡时如果有特殊原因必须设置 否则默认为被攻击死亡
    /// </summary>
    /// <param name="reason"></param>
    public void SetDeathReason(DamageState reason)
    {
        DeathReason = reason;
    }

    public virtual void SetHPMAX(int hpMax)
    {
        HPMAX = hpMax;
    }

    /// <summary>
    ///  改变战斗状态
    /// </summary>
    /// <param name="isInBattle"></param>
    /// <param name="isForce">是否强制改变</param>
    /// <returns>改变成功或者失败 状态没变为失败 主要给子类覆写使用</returns>
    public virtual bool ChangeIsBattle(bool isInBattle, bool isForce = false)
    {
        if (IsInBattle == isInBattle && !isForce)
        {
            return false;
        }

        IsInBattle = isInBattle;
        RefEntity.EntityEvent.ChangeIsBattle?.Invoke(IsInBattle);
        return true;
    }

    /// <summary>
    /// 添加一个战斗状态
    /// </summary>
    /// <param name="key"> 状态key</param>
    /// <returns>/returns>
    public void AddBattleState(BattleDefine.eBattleState key)
    {
        if (_battleStateMap.TryGetValue(key, out int num))
        {
            _battleStateMap[key] = num + 1;
        }
        else
        {
            _battleStateMap.Add(key, 1);
        }
    }

    /// <summary>
    /// 删除一个战斗状态
    /// </summary>
    /// <param name="key"> 状态key</param>
    /// <returns>/returns>
    public void RemoveBattleState(BattleDefine.eBattleState key)
    {
        if (_battleStateMap.TryGetValue(key, out int num))
        {
            _battleStateMap[key] = num - 1;
            if (_battleStateMap[key] <= 0)
            {
                _ = _battleStateMap.Remove(key);
            }
        }
        else
        {
            Log.Error($"RemoveBattleState Not Find State = {key}");
        }
    }

    /// <summary>
    /// 是否存在战斗状态
    /// </summary>
    /// <param name="key"> 状态key</param>
    /// <returns>/returns>
    public bool HasBattleState(BattleDefine.eBattleState key)
    {
        return _battleStateMap.ContainsKey(key);
    }

    /// <summary>
    /// 是否存活
    /// </summary>
    public bool IsLive()
    {
        return Status == EntityStatus.Live;
    }

    private int GetHpValue()
    {
        if (_hpAttribute == null)
        {
            _hpAttribute = RefEntity.EntityAttributeData.GetAttribute(eAttributeType.HP);
        }
        return _hpAttribute.Value;
    }

    private void SetHpValue(int value)
    {
        if (_hpAttribute == null)
        {
            _hpAttribute = RefEntity.EntityAttributeData.GetAttribute(eAttributeType.HP);
        }
        //基础属性没变化
        if (_hpAttribute.BaseValue == value)
        {
            return;
        }
        _ = _hpAttribute.SetBase(value);
        RefEntity.EntityAttributeData.IsNetDirty = true;
        RefEntity.EntityEvent.EntityAttributeUpdate?.Invoke(eAttributeType.HP, value);
    }

    protected int GetValue(eAttributeType type)
    {
        return RefEntity.EntityAttributeData.GetValue(type);
    }

    protected float GetRealValue(eAttributeType type)
    {
        return RefEntity.EntityAttributeData.GetRealValue(type);
    }

    protected void SetBaseValue(eAttributeType type, int value)
    {
        RefEntity.EntityAttributeData.SetBaseValue(type, value);
    }

    /// <summary>
    /// 返回移动速度
    /// </summary>
    protected float GetMoveSpeedValue()
    {
        float speed = GetValue(eAttributeType.MoveSpd) * MathUtilCore.CM2M;
        if (_playerAreaRecord != null && _playerAreaRecord.CurArea == eSceneArea.Home)
        {
            speed += GetValue(eAttributeType.HomeExtraMoveSpd) * MathUtilCore.CM2M;
        }
        return speed;
    }

    /// <summary>
    /// 检测移动
    /// </summary>
    public bool CheckCanMove()
    {
        foreach (BattleDefine.eBattleState state in BattleDefine.BATTLE_STATE_CANNOT_MOVE_LIST)
        {
            if (_battleStateMap.ContainsKey(state))
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 检测释放技能
    /// </summary>

    public bool CheckCanSkill()
    {
        foreach (BattleDefine.eBattleState state in BattleDefine.BATTLE_STATE_CANNOT_SKILL_LIST)
        {
            if (_battleStateMap.ContainsKey(state))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 设置随机种子
    /// </summary>
    /// <param name="seed"></param>
    public void SetRandomSeed(int seed)
    {
        RandomSeed = seed;
    }

    /// <summary>
    ///  改变实体状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="sync">是否同步</param>
    /// <returns>改变成功或者失败 状态没变为失败 主要给子类覆写使用</returns>
    public virtual bool ChangeStatus(EntityStatus status, bool sync = false)
    {
        if (Status == status)
        {
            return false;
        }
        Status = status;
        RefEntity.EntityEvent.ChangeEntityStatus?.Invoke();
        return true;
    }
}