/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 持续伤害效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEDotDamageCore.cs
* 
*/

using GameMessageCore;
public class SEDotDamageCore : SkillEffectBase
{
    public override bool IsUpdate => true;
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
        float damageCoefficient = 1;
        if (EffectCfg.Parameters != null && EffectCfg.Parameters.Length > 0)
        {
            damageCoefficient = EffectCfg.Parameters[0] * MathUtilCore.I2T;
        }
        DamageEffect effect = new();
        EntityBattleDataCore targetBattleData = targetEntity.BattleDataCore;
        DamageData damage = SkillDamage.DamageCalculation(fromEntity.EntityAttributeData, targetEntity.EntityAttributeData, fromEntity.BattleDataCore.Level, targetEntity.BattleDataCore.Level, damageCoefficient);
        effect.DamageValue = damage;
        effect.DamageValue.CurrentInt = targetBattleData.HP + damage.DeltaInt;
        return effect;
    }
}