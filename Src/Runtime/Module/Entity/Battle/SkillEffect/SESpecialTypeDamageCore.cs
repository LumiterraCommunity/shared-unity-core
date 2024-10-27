using UnityGameFramework.Runtime;

/// <summary>
/// 特殊类型伤害效果球 比如押镖伤害、各元素伤害等
/// </summary>
public class SESpecialTypeDamageCore : SENormalDamageCore
{
    public override void OnAdd()
    {
        base.OnAdd();

        if (EffectCfg.Parameters.Length >= 3)
        {
            DamageType = TableUtil.ConvertToBitEnum<eDamageType>(EffectCfg.Parameters[2]);
        }
        else
        {
            Log.Error("SESpecialTypeDamageCore OnAdd EffectCfg.Parameters.Length < 3");
            DamageType = eDamageType.Unknown;
        }
    }
}