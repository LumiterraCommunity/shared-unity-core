
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 减免伤害 每次受到伤害不会超过参数配置的比例
/// </summary>
public class SEMaxDamageToMaxHpRatioCore : SkillEffectBase, ISEDamageReduction
{
    private float _maxHpRatio = -1;//最大血量比例 0-1 负数代表未初始化

    public override void Clear()
    {
        _maxHpRatio = -1;

        base.Clear();
    }

    public override void OnAdd()
    {
        base.OnAdd();

        if (EffectCfg.Parameters.Length > 0)
        {
            _maxHpRatio = EffectCfg.Parameters[0] * TableDefine.THOUSANDTH_2_FLOAT;
        }
        else
        {
            Log.Error($"SEMaxDamageToMaxHpRatioCore OnAdd EffectCfg.Parameters.Length <= 0,EffectCfg:{EffectCfg.Id}");
        }
    }

    public int ReductionDamage(EntityBase fromEntity, int deltaInt)
    {
        if (RefEntity.BattleDataCore == null)
        {
            return deltaInt;
        }

        if (_maxHpRatio < 0)//异常
        {
            return deltaInt;
        }

        int maxDamage = -Mathf.RoundToInt(RefEntity.BattleDataCore.HPMAX * _maxHpRatio);
        return Mathf.Max(deltaInt, maxDamage);//都是负数 所以max
    }
}