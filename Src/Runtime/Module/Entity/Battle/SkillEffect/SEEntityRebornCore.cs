/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 实体重生效果球，不能作用于灵魂状态实体
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEEntityRebornCore.cs
* 
*/

using GameMessageCore;

public class SEEntityRebornCore : SkillEffectBase
{
    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        float hpCoefficient = 1;
        if (EffectCfg.Parameters != null && EffectCfg.Parameters.Length > 0)
        {
            hpCoefficient = EffectCfg.Parameters[0] * MathUtilCore.I2T;
        }
        DamageEffect effect = new();
        int reviveHp = 0;
        //灵魂状态不能复活
        if (!targetEntity.BattleDataCore.IsInSoul)
        {
            reviveHp = (int)(targetEntity.BattleDataCore.HPMAX * hpCoefficient);
        }
        effect.IntValue = reviveHp;
        return effect;
    }
}