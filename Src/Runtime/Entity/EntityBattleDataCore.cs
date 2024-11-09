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
using UnityEngine;
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
    public int HP { get => GetValue(eAttributeType.HP); protected set => SetBaseValue(eAttributeType.HP, value); }
    /// <summary>
    /// 最大血量
    /// </summary>
    /// <value></value>
    public int HPMAX { get => GetValue(eAttributeType.MaxHP); protected set => SetBaseValue(eAttributeType.MaxHP, value); }
    /// <summary>
    /// 当前白血量
    /// </summary>
    /// <value></value>
    public int WhiteHP { get => GetValue(eAttributeType.WhiteHP); protected set => SetBaseValue(eAttributeType.WhiteHP, value); }
    /// <summary>
    /// 最大白血量
    /// </summary>
    /// <value></value>
    public int WhiteHPMAX { get => GetValue(eAttributeType.MaxWhiteHP); protected set => SetBaseValue(eAttributeType.MaxWhiteHP, value); }
    /// <summary>
    /// 总血量 包括白血量 护盾等所有血量
    /// </summary>
    public int TotalHP => HP + WhiteHP;
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
    public float Level { get => GetLevelValue(); protected set => SetLevelValue(value); }
    public Dictionary<eAttributeType, IntAttribute> AttributeMap { get; private set; } = new(); //优化获取属性性能
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
    /// <summary>
    /// 设置血量
    /// </summary>
    public virtual void SetHP(int hp, bool isForce = false)
    {
        HP = Math.Clamp(hp, 0, HPMAX);
    }

    /// <summary>
    /// 设置死亡原因 在死亡时如果有特殊原因必须设置 否则默认为被攻击死亡
    /// </summary>
    /// <param name="reason"></param>
    public void SetDeathReason(DamageState reason)
    {
        DeathReason = reason;
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

    private IntAttribute GetAttribute(eAttributeType type)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            return attribute;
        }
        else
        {
            attribute = RefEntity.EntityAttributeData.GetAttribute(type);
            AttributeMap.Add(type, attribute);
            return attribute;
        }
    }

    protected void SetBaseValue(eAttributeType type, int value)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            RefEntity.EntityAttributeData.SetBaseValue(attribute, type, value);
        }
        else
        {
            attribute = GetAttribute(type);
            RefEntity.EntityAttributeData.SetBaseValue(attribute, type, value);
        }
    }

    protected void SetLevelValue(float value)
    {
        SetBaseValue(eAttributeType.CombatLv, (int)value);
        SetBaseValue(eAttributeType.ExtThousLv, (int)((value - (int)value) * MathUtilCore.T2I));
    }

    protected float GetLevelValue()
    {
        return GetRealValue(eAttributeType.CombatLv) + GetRealValue(eAttributeType.ExtThousLv);
    }
    protected int GetValue(eAttributeType type)
    {
        return GetAttribute(type).Value;
    }

    protected float GetRealValue(eAttributeType type)
    {
        return GetAttribute(type).RealValue;
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

    /// <summary>
    /// 改变血量
    /// </summary>
    /// <param name="changeHp"></param>
    public virtual void ChangeHP(int changeHp)
    {
        (int hp, int whiteHP) = CalChangeHP(changeHp, HP, WhiteHP);
        SetHP(hp);
        SetWhiteHP(whiteHP);
    }

    /// <summary>
    /// 计算血量修改 返回实际修改的血量和白血量
    /// </summary>
    /// <param name="changeHp"></param>
    /// <returns> 返回实际修改的血量和白血量</returns>
    public virtual (int, int) CalChangeHP(int changeHp, int hp, int whiteHp)
    {
        whiteHp = Math.Clamp(whiteHp, 0, WhiteHPMAX);
        if (changeHp > 0)
        {
            hp += changeHp;
            return (hp, whiteHp);
        }
        else
        {
            int blockHP = whiteHp + changeHp;
            if (blockHP >= 0)
            {
                return (hp, blockHP);
            }
            else
            {
                return (hp + blockHP, 0);
            }
        }
    }
    public virtual void SetWhiteHP(int whiteHP)
    {
        WhiteHP = Math.Clamp(whiteHP, 0, WhiteHPMAX);
    }

    /// <summary>
    /// hp和whiteHp自动适配最大值 会按照新的最大值变化比例设置血量 需要给出旧的最大值
    /// 可以修复max变小hp超过上限 换装后max变小变大后hp回不来的问题
    /// </summary>
    public virtual void HpAutoAdaptMax(int oldMaxHP, int oldMaxWhiteHP)
    {
        if (oldMaxHP > 0 && oldMaxHP != HPMAX)
        {
            HP = Mathf.RoundToInt(HP * (float)HPMAX / oldMaxHP);
            HP = Mathf.Min(HP, HPMAX);
        }
        if (oldMaxWhiteHP > 0 && oldMaxWhiteHP != WhiteHPMAX)
        {
            WhiteHP = Mathf.RoundToInt(WhiteHP * (float)WhiteHPMAX / oldMaxWhiteHP);
            WhiteHP = Mathf.Min(WhiteHP, WhiteHPMAX);
        }
    }
}