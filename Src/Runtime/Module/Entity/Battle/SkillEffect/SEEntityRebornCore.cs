/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 实体重生效果球，不能作用于灵魂状态实体
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEEntityRebornCore.cs
* 
*/

using GameMessageCore;

public class SEEntityRebornCore : SkillEffectBase
{
    public override void Start()
    {
        base.Start();
        //创建效果球时就判断是否可以复活，所以这里哪怕是灵魂状态也可以复活，
        if (EffectData.IntValue > 0)
        {
            RefEntity.BattleDataCore.SetHP(EffectData.IntValue, true);
            _ = RefEntity.BattleDataCore.ChangeIsSoul(false);
            RefEntity.EntityEvent.EntityBeReborn?.Invoke();
        }
    }
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