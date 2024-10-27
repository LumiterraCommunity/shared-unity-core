
using UnityGameFramework.Runtime;

/// <summary>
///  决定是否能够接受某些类型的伤害 挂在防御者身上 没有挂只接受普通伤害 挂了就只能接受这里配置的伤害
/// </summary>
public class SEAcceptDamageTypeCore : SkillEffectBase
{
    //接受的伤害类型
    private eDamageType _supportDamageTypes = eDamageType.Unknown;

    public override void Clear()
    {
        _supportDamageTypes = eDamageType.Unknown;

        base.Clear();
    }

    public override void OnAdd()
    {
        base.OnAdd();

        if (EffectCfg.Parameters.Length > 0)
        {
            _supportDamageTypes = TableUtil.ConvertToBitEnum<eDamageType>(EffectCfg.Parameters);
        }
        else
        {
            Log.Error($"SEMaxDamageToMaxHpRatioCore OnAdd EffectCfg.Parameters.Length <= 0,EffectCfg:{EffectCfg.Id}");
        }
    }

    /// <summary>
    /// 检测是否能接受这种伤害类型 参数是配置表中的伤害类型 内部会位运算转成枚举
    /// </summary>
    internal bool CheckAcceptDamageType(int damageType)
    {
        eDamageType eType = TableUtil.ConvertToBitEnum<eDamageType>(damageType);
        return CheckAcceptDamageType(eType);
    }

    /// <summary>
    /// 检测是否能接受这种伤害类型
    /// </summary>
    internal bool CheckAcceptDamageType(eDamageType damageType)
    {
        return (_supportDamageTypes & damageType) != 0;
    }
}