using System.Collections.Generic;
using System;
using UnityGameFramework.Runtime;
/// <summary>
/// 血怒效果球
/// 血量越低，伤害越高
/// </summary>
public class SEBloodRageCore : SkillEffectBase
{
    private List<IntAttributeModifier> _modifiers;
    public override void OnAdd()
    {
        base.OnAdd();
        _modifiers = new();
        RefEntity.EntityEvent.EntityAttributeUpdate += OnEntityAttributeUpdate;
        float hpLostPercent = GetLostHpPercent();
        if (!hpLostPercent.ApproximatelyEquals(0))
        {
            UpdateAttrByHP();
        }
    }

    public override void OnRemove()
    {
        RefEntity.EntityEvent.EntityAttributeUpdate -= OnEntityAttributeUpdate;
        ClearModifier();
        base.OnRemove();
    }

    public override void Clear()
    {
        _modifiers.Clear();
        _modifiers = null;
        base.Clear();
    }

    private void ClearModifier()
    {
        if (_modifiers != null)
        {
            foreach (IntAttributeModifier modifier in _modifiers)
            {
                RefEntity.EntityAttributeData.RemoveModifier(modifier);
            }
            _modifiers.Clear();
        }
    }

    private void OnEntityAttributeUpdate(eAttributeType type, int value)
    {
        if (type == eAttributeType.HP)
        {
            UpdateAttrByHP();
        }
    }

    public void UpdateAttrByHP()
    {
        if (RefEntity.EntityAttributeData == null)
        {
            return;
        }

        if (EffectCfg.Parameters2.Length == 0)
        {
            return;
        }

        for (int i = 0; i < EffectCfg.Parameters2.Length; i++)
        {
            int[] dataRow = EffectCfg.Parameters2[i];
            if (dataRow.Length < 3)
            {
                Log.Error($"SEBloodRageCore 参数(Parameters2)配置错误，参数数量不足3个，参数数量 = {dataRow.Length}，参数行 = {i}");
                continue;
            }

            ClearModifier();

            eAttributeType attributeType = GetAttributeType(dataRow[0]);
            eModifierType modifyType = (eModifierType)dataRow[1];
            float factor = GetModifyFactor(dataRow[2], modifyType);
            int value = (int)(GetLostHpPercent() * factor);
            if (value != 0)
            {
                IntAttributeModifier modifier = RefEntity.EntityAttributeData.AddModifier(attributeType, modifyType, value);
                _modifiers.Add(modifier);
            }
        }
    }

    private float GetLostHpPercent()
    {
        if (RefEntity.BattleDataCore == null)
        {
            return 0f;
        }

        float maxHP = RefEntity.BattleDataCore.HPMAX;
        float curHP = RefEntity.BattleDataCore.HP;

        float lostHpPercent = (maxHP - curHP) / maxHP * 100f;
        return Math.Clamp(lostHpPercent, 0f, 100f);//血量可能是负数，这里做一下限制
    }

    private eAttributeType GetAttributeType(int cfgData)
    {
        return (eAttributeType)cfgData;
    }

    private float GetModifyFactor(int cfgData, eModifierType modifyType)
    {
        if (modifyType is eModifierType.PctAdd or eModifierType.FinalPctAdd)
        {
            return cfgData;
        }
        else
        {
            return cfgData / IntAttribute.PERCENTAGE_FLAG;
        }
    }
}