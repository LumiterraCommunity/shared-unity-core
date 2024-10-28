/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 持续伤害效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEDotDamageCore.cs
* 
*/

using GameMessageCore;
using UnityGameFramework.Runtime;
public class SEDotDamageCore : SkillEffectBase
{
    public override bool IsUpdate => true;

    private eDamageType _damageTypes = eDamageType.Unknown;//产生的伤害类 复合型

    public override void OnAdd()
    {
        base.OnAdd();

        if (EffectCfg.Parameters.Length >= 3)
        {
            _damageTypes = (eDamageType)EffectCfg.Parameters[2];
        }
        else//没配置就是普通伤害
        {
            _damageTypes = eDamageType.Normal;
        }
    }

    public override void Clear()
    {
        _damageTypes = eDamageType.Unknown;

        base.Clear();
    }

    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }

        if (!SkillDamage.CheckTargetCanAcceptDamage(targetEntity, _damageTypes))
        {
            return false;
        }

        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
    }

    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        float damageValue = 1;
        if (EffectCfg.Parameters != null && EffectCfg.Parameters.Length > 0)
        {
            damageValue = EffectCfg.Parameters[0] * MathUtilCore.I2T;
        }
        eDotDamageType damageType = eDotDamageType.Normal;
        if (EffectCfg.Parameters.Length > 1)
        {
            damageType = (eDotDamageType)EffectCfg.Parameters[1];
        }
        DamageEffect effect = new();
        EntityBattleDataCore targetBattleData = targetEntity.BattleDataCore;
        DamageData damage;

        if (damageType == eDotDamageType.Normal)
        {
            damage = SkillDamage.DamageCalculation(fromEntity.EntityAttributeData, targetEntity.EntityAttributeData, damageValue);
        }
        else if (damageType == eDotDamageType.Fixed)
        {
            damage = SkillDamage.MakeDamageData(DamageState.Normal, targetBattleData.HP, targetBattleData.WhiteHP, -(int)damageValue);
        }
        else if (damageType is eDotDamageType.Percent or eDotDamageType.MaxPercent)
        {
            int value = damageType == eDotDamageType.Percent ? targetBattleData.HP : targetBattleData.HPMAX;
            damage = SkillDamage.MakeDamageData(DamageState.Normal, targetBattleData.HP, targetBattleData.WhiteHP, -(int)(value * damageValue));
        }
        else
        {
            Log.Error("SEDotDamage Unknown sustained damage Id: " + EffectCfg.Id + " damageType: " + damageType);
            damage = SkillDamage.MakeDamageData(DamageState.Normal, targetBattleData.HP, targetBattleData.WhiteHP, 0);
        }

        //防御方伤害减免
        damage.DeltaInt = SkillDamage.DamageReduction(fromEntity, targetEntity, damage.DeltaInt);

        effect.DamageValue = damage;
        effect.DamageValue.CurrentInt = targetBattleData.HP + damage.DeltaInt;
        effect.DamageValue.WhiteInt = targetBattleData.WhiteHP;
        return effect;
    }
}