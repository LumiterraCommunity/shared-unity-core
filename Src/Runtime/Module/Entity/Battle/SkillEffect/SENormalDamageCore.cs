/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 普通伤害效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SENormalDamageCore.cs
* 
*/

using GameMessageCore;
public class SENormalDamageCore : SkillEffectBase
{
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

        eDamageType damageTypes = EffectCfg.Parameters.Length >= 3 ? (eDamageType)EffectCfg.Parameters[2] : eDamageType.Normal;
#if UNITY_EDITOR
        damageTypes |= eDamageType.EscortWagon;
#endif
        if (!SkillDamage.CheckTargetCanAcceptDamage(targetEntity, damageTypes))
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

        //场景伤害
        if (SkillUtil.IsSceneDeath(EffectData.DamageValue.DmgState))
        {
            if (RefEntity.BattleDataCore.IsLive())
            {
                RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(BattleDefine.SCENE_DAMAGE_ENTITY_ID, -EffectData.DamageValue.DeltaInt);
            }

            RefEntity.BattleDataCore.SetHP(EffectData.DamageValue.CurrentInt);
            RefEntity.BattleDataCore.SetWhiteHP(EffectData.DamageValue.WhiteInt);
            if (!RefEntity.BattleDataCore.IsLive())
            {
                RefEntity.BattleDataCore.SetDeathReason(EffectData.DamageValue.DmgState);
            }
            return;
        }

        if (RefEntity.BattleDataCore != null)
        {
            bool isLive = RefEntity.BattleDataCore.IsLive();
            RefEntity.BattleDataCore.SetHP(EffectData.DamageValue.CurrentInt);
            RefEntity.BattleDataCore.SetWhiteHP(EffectData.DamageValue.WhiteInt);
            if (EffectData.DamageValue.DeltaInt < 0 && isLive)
            {
                RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(FromID, -EffectData.DamageValue.DeltaInt);
            }

        }
        RefEntity.EntityEvent.EntityBeHit?.Invoke(SkillID);
    }

    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        float damageCoefficient = 1;
        if (EffectCfg.Parameters != null && EffectCfg.Parameters.Length > 0)
        {
            damageCoefficient = EffectCfg.Parameters[0] * MathUtilCore.I2T;
        }
        DamageEffect effect = new();
        DamageData damage = SkillDamage.DamageCalculation(fromEntity.EntityAttributeData, targetEntity.EntityAttributeData, damageCoefficient, inputData.InputRandom);

        //防御方伤害减免
        damage.DeltaInt = SkillDamage.DamageReduction(fromEntity, targetEntity, damage.DeltaInt);

        effect.DamageValue = damage;
        (int hp, int whiteHP) = targetEntity.BattleDataCore.CalChangeHP(damage.DeltaInt, targetEntity.BattleDataCore.HP, targetEntity.BattleDataCore.WhiteHP);
        effect.DamageValue.CurrentInt = hp;
        effect.DamageValue.WhiteInt = whiteHP;
        return effect;
    }
}