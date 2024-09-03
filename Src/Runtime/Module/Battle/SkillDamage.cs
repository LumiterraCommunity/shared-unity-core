using System;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SkillDamage
{
    // private const int DAMAGE_WAVE_RANGE = 2;  // 策划约定，伤害值需要附加 -2% ～ 2% 的浮动变化 
    private const float MIN_DAMAGE = 1f;//最低伤害
    public static DamageData MakeDamageData(DamageState state, int defenderCurHp, int whiteHp, int damage)
    {
        return new DamageData()
        {
            DmgState = state,
            CurrentInt = defenderCurHp,
            WhiteInt = whiteHp,
            DeltaInt = damage,
        };
    }
    /// <summary>
    /// 检测命中
    /// </summary>
    /// <param name="attackerBattleData"></param>
    /// <param name="defenderBattleData"></param>
    /// <returns>DamageData</returns>
    public static bool CheckHit(EntityBattleDataCore attackerBattleData, EntityBattleDataCore defenderBattleData, InputRandomData inputRandom = null)
    {
        //无敌状态无法命中
        if (defenderBattleData.HasBattleState(BattleDefine.eBattleState.Invincible))
        {
            return false;
        }
        float realHitRate = (float)(1.0 + (attackerBattleData.HitRate - defenderBattleData.MissRate) / 100.0f);
        int realHitRatePercent = (int)(realHitRate * 100.0f);
        int randValue;
        if (inputRandom == null)
        {
            randValue = UnityEngine.Random.Range(0, 100);
        }
        else
        {
            randValue = inputRandom.HitValue;
        }
        return randValue <= realHitRatePercent;
    }

    /// <summary>
    /// 计算技能伤害，damageData.DeltaInt 为真实伤害数数值，负数为减血，正数为加血
    /// </summary>
    /// <param name="coefficient">伤害系数</param>
    /// <returns>DamageData</returns>
    public static DamageData DamageCalculation(EntityAttributeData fromAttribute, EntityAttributeData toAttribute, float coefficient = 1, InputRandomData inputRandom = null)
    {
        int defHp = toAttribute.GetValue(eAttributeType.HP);
        int defWhiteHp = toAttribute.GetValue(eAttributeType.WhiteHP);

        if (fromAttribute == null || toAttribute == null)
        {
            Log.Error($"DamageCalculation fromAttribute or toAttribute is null,form:{fromAttribute}");
            return MakeDamageData(DamageState.Normal, defHp, defWhiteHp, 0);
        }

        (float damage, bool crit) = CalculateEnemyDamage(fromAttribute, toAttribute, inputRandom);

        float realDamage = damage * coefficient;
        return MakeDamageData(crit ? DamageState.Crit : DamageState.Normal, defHp, defWhiteHp, -UnityEngine.Mathf.RoundToInt(realDamage));
    }

    /// <summary>
    /// 计算对敌人的伤害 怪物 boss等
    /// </summary>
    /// <param name="fromAttribute"></param>
    /// <param name="toAttribute"></param>
    /// <returns></returns>
    public static (float damage, bool crit) CalculateEnemyDamage(EntityAttributeData fromAttribute, EntityAttributeData toAttribute, InputRandomData inputRandom = null)
    {
        if (fromAttribute == null || toAttribute == null)
        {
            Log.Error($"CalculateEnemyDamage fromAttribute or toAttribute is null,form:{fromAttribute}");
            return (0, false);
        }

        float fromLevel = CalculateEntityDamageLevel(fromAttribute.RefEntity, true);
        float toLevel = CalculateEntityDamageLevel(toAttribute.RefEntity, false);

        TableEnemyDamageAttribute attributeClassify = EntityAttributeTable.Inst.GetDamageAttributeClassify<TableEnemyDamageAttribute>(HomeDefine.eAction.AttackEnemy);

        float baseDamage = CalculateBaseDamage(fromAttribute.GetRealValue(attributeClassify.Att), toAttribute.GetRealValue(attributeClassify.Def), fromLevel, toLevel);

        (float coreDamage, bool crit) = CalculateCoreDamage(baseDamage, attributeClassify, fromAttribute, inputRandom);

        float res = coreDamage * (1 + toAttribute.GetRealValue(attributeClassify.Vulnerable));

        //PVP伤害有额外系数
        if (EntityUtilCore.EntityTypeIsPlayerUnit(fromAttribute.RefEntity.BaseData.Type) && EntityUtilCore.EntityTypeIsPlayerUnit(toAttribute.RefEntity.BaseData.Type))
        {
            res *= TableUtil.GetGameValue(eGameValueID.PVPDamageRate).Value * TableDefine.THOUSANDTH_2_FLOAT;
        }

        res = Math.Max(MIN_DAMAGE, res);

        return (res, crit);
    }

    /// <summary>
    /// 计算敌人实体伤害等级 这里只处理Enemy中的计算 不参与家园 这些等级会根据实体类型不同获取 返回小数用于精确计算
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="isFromRole">是否攻击者</param>
    /// <returns></returns>
    private static float CalculateEntityDamageLevel(EntityBase entity, bool isFromRole)
    {
        if (entity == null)
        {
            Log.Error($"CalculateEntityDamageLevel entity is null");
            return 0;
        }

        //角色和宠物取装备等级平均值
        if (EntityUtilCore.EntityTypeIsPlayer(entity.BaseData.Type) || EntityUtilCore.EntityTypeIsPet(entity.BaseData.Type))
        {
            if (isFromRole)//攻击者用战斗等级
            {
                return entity.GetComponent<EntityAvatarDataCore>().GetAbilityLevel(eTalentType.battle);
            }
            else//防御者使用当前等级 可以使采集者更肉类似
            {
                return entity.GetComponent<EntityAvatarDataCore>().GetCurAbilityLevel();
            }
        }
        else
        {
            return entity.BattleDataCore.Level;
        }
    }

    /// <summary>
    /// 计算家园动作的伤害 砍树 挖矿等
    /// </summary>
    /// <param name="action"></param>
    /// <param name="fromAttribute">发起攻击者属性 不能为空</param>
    /// <param name="toAttribute">防御方属性 为空时代表没有防御相关属性</param>
    /// <param name="skillDamageRate">技能伤害倍率 由技能决定的</param>
    /// <returns></returns>
    public static (float damage, bool crit) CalculateHomeDamage(HomeDefine.eAction action, EntityAttributeData fromAttribute, EntityAttributeData toAttribute, float skillDamageRate, InputRandomData seedData, float fromLevel, float toLevel)
    {
        if (fromAttribute == null)
        {
            Log.Error($"CalculateHomeDamage fromAttribute is null");
            return (0, false);
        }

        if ((action & HomeDefine.NEED_CALCULATE_DAMAGE_ACTION_MASK) == 0)
        {
            Log.Error($"CalculateHomeDamage action:{action} is not support calculate damage");
            return (0, false);
        }

        TableHomeDamageAttribute attributeClassify = EntityAttributeTable.Inst.GetDamageAttributeClassify<TableHomeDamageAttribute>(action);

        float toDef = toAttribute != null ? toAttribute.GetRealValue(attributeClassify.Def) : 0;
        float baseDamage = CalculateBaseDamage(fromAttribute.GetRealValue(attributeClassify.Att), toDef, fromLevel, toLevel);

        (float coreDamage, bool crit) = CalculateCoreDamage(baseDamage, attributeClassify, fromAttribute, seedData);

        float res = coreDamage;// * MathF.Min(1, fromAttribute.GetRealValue(attributeClassify.AvailableLv) / toLevel); 数值改造时去掉了
        res *= skillDamageRate;
        res = Math.Max(MIN_DAMAGE, res);

        return (res, crit);
    }

    /// <summary>
    /// 计算基础伤害
    /// </summary>
    /// <param name="atk">战斗或者对草树的攻击力</param>
    /// <param name="def">战斗或者采集物防御</param>
    /// <param name="levelAtk">攻击者等级 不一定是人物最高等级 要看上层如何计算</param>
    /// <param name="levelDef">防御方等级</param>
    /// <returns></returns>
    private static float CalculateBaseDamage(float atk, float def, float levelAtk, float levelDef)
    {
        if (atk.ApproximatelyEquals(0) && def.ApproximatelyEquals(0))//待会分母不能为0
        {
            return 0;
        }

        float res = Mathf.Pow(atk, 2) / (atk + def) * Mathf.Pow(2, (levelAtk - levelDef) / 2);
        if (res < 0)
        {
            Log.Error($"Calculate Base Damage error, res:{res} atk:{atk} def:{def} levelAtk:{levelAtk} levelDef:{levelDef}");
            return 0;
        }
        return res;
    }

    /// <summary>
    /// 计算核心伤害值 并返回是否暴击
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="coreDamageClassify"></param>
    /// <param name="fromAttribute"></param>
    /// <returns></returns>
    private static (float damage, bool crit) CalculateCoreDamage(float baseDamage, TableCoreDamageAttribute coreDamageClassify, EntityAttributeData fromAttribute, InputRandomData inputRandom = null)
    {
        int critRate = (int)(fromAttribute.GetRealValue(coreDamageClassify.CritRate) * 1000);
        int realCritExtra;
        if (inputRandom == null)
        {
            realCritExtra = UnityEngine.Random.Range(0, 1000) < critRate ? 1 : 0;
        }
        else
        {
            realCritExtra = inputRandom.CritValue < critRate ? 1 : 0;
        }
        float res = baseDamage * (1 + (realCritExtra * fromAttribute.GetRealValue(coreDamageClassify.CritDmg)));
        res *= 1 + fromAttribute.GetRealValue(coreDamageClassify.DmgBonus);
        return (res, realCritExtra == 1);
    }

    /// <summary>
    /// 创建特殊伤害效果
    /// </summary>
    /// <returns></returns>
    public static DamageEffect CreateSpecialDamageEffect(DamageState type, int finalHp, int finalWhiteHp, int deltaHp)
    {
        DamageEffect effect = new()
        {
            DamageValue = MakeDamageData(type, finalHp, finalWhiteHp, deltaHp),
            EffectType = (DamageEffectId)TableDefine.DAMAGE_EFFECT_ID
        };
        return effect;
    }
}