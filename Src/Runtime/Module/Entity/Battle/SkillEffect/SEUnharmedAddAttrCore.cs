
using UnityEngine;
using UnityGameFramework.Runtime;
/// <summary>
/// 不受伤害时增加属性
/// </summary>
public class SEUnharmedAddAttrCore : SkillEffectBase
{
    public override bool IsUpdate => true;
    /// <summary>
    /// 当前效果是否生效
    /// </summary>
    private bool _enable = false;
    /// <summary>
    /// 收到伤害的时间记录
    /// </summary>
    private float _getHurtTimeRecord;
    /// <summary>
    /// 触发效果的时间
    /// </summary>
    private float _triggerTime = float.MaxValue;
    /// <summary>
    /// 属性修改器
    /// </summary>
    private IntAttributeModifier _modifier;
    public override void OnAdd()
    {
        base.OnAdd();


        _getHurtTimeRecord = Time.realtimeSinceStartup;
        if (EffectCfg.Parameters.Length > 0)
        {
            _triggerTime = EffectCfg.Parameters[0];
        }
        RefEntity.EntityEvent.EntityBattleAddDamage += OnGetHurt;
    }

    public override void OnRemove()
    {
        RefEntity.EntityEvent.EntityBattleAddDamage -= OnGetHurt;
        DisableEffect();
        base.OnRemove();
    }

    private void OnGetHurt(long fromId, int value)
    {
        _getHurtTimeRecord = Time.realtimeSinceStartup;
        DisableEffect();
    }

    public override void Update()
    {
        base.Update();

        float deltaTime = (Time.realtimeSinceStartup - _getHurtTimeRecord) * TimeUtil.S2MS;
        if (!_enable && deltaTime >= _triggerTime)
        {
            EnableEffect();
        }
    }

    private void EnableEffect()
    {
        if (_enable)
        {
            return;
        }
        _enable = true;

        if (EffectCfg.Parameters2.Length == 0)
        {
            Log.Error("SeUnharmedToAddAttr OnAdd Error: Parameters2 Length is 0");
            return;
        }

        for (int i = 0; i < EffectCfg.Parameters2.Length; i++)
        {
            int[] rowData = EffectCfg.Parameters2[i];
            if (rowData.Length < 3)
            {
                Log.Error($"SeUnharmedToAddAttr OnAdd Error: Parameters2 Length is {rowData.Length}");
                continue;
            }

            eAttributeType attrType = (eAttributeType)rowData[0];
            eModifierType modifyType = (eModifierType)rowData[1];
            int value = (int)GetModifyFactor(rowData[2], modifyType);
            if (value != 0)
            {
                _modifier = RefEntity.EntityAttributeData.AddModifier(attrType, modifyType, value);
            }
        }
    }

    private void DisableEffect()
    {
        if (!_enable)
        {
            return;
        }
        _enable = false;

        if (_modifier != null)
        {
            RefEntity.EntityAttributeData.RemoveModifier(_modifier);
            _modifier = null;
        }
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