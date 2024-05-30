using System.Collections.Generic;
using GameFramework.DataTable;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 管理属性数据的组件
/// </summary>
public class AttributeDataCpt : MonoBehaviour
{
    public Dictionary<eAttributeType, IntAttribute> AttributeMap { get; private set; } = new();
    private readonly Dictionary<eAttributeType, int> _defaultMap = new();

    private readonly List<AttributeData> _netAttributeBaseDataList = new();
    public bool IsNetDirty = true;
    private bool _isDestroy = false;
    protected virtual void Awake()
    {
        IDataTable<DREntityAttribute> dtAircraft = GFEntryCore.DataTable.GetDataTable<DREntityAttribute>();
        DREntityAttribute[] attributes = dtAircraft.GetAllDataRows();
        for (int i = 0; i < attributes.Length; i++)
        {
            _defaultMap.Add((eAttributeType)attributes[i].Id, attributes[i].DefaultValue);
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (KeyValuePair<eAttributeType, IntAttribute> item in AttributeMap)
        {
            item.Value.Dispose();
        }
        AttributeMap.Clear();
        _isDestroy = true;
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    public int GetValue(eAttributeType type)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            return attribute.Value;
        }
        //默认值
        if (_defaultMap.TryGetValue(type, out int value))
        {
            return value;
        }
        Log.Error($"EntityAttributeData GetValue Not Find Attribute Type = {type}");
        return 0;
    }

    /// <summary>
    /// 获取实际属性值, 根据值类型计算后的值
    /// </summary>
    public float GetRealValue(eAttributeType type)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            return attribute.RealValue;
        }
        //默认值
        if (_defaultMap.TryGetValue(type, out int value))
        {
            float coefficient = TableUtil.GetAttributeCoefficient(type);
            return value * coefficient;
        }
        Log.Error($"EntityAttributeData GetRealValue Not Find Attribute Type = {type}");
        return 0;
    }

    /// <summary>
    /// 获取属性
    /// </summary>
    public IntAttribute GetAttribute(eAttributeType type)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            return attribute;
        }
        else
        {
            attribute = CreateAttribute(type);
            AttributeMap.Add(type, attribute);
            return attribute;
        }
    }

    /// <summary>
    /// 设置属性基础值
    /// </summary>
    public void SetBaseValue(eAttributeType type, int value)
    {
        if (!AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            attribute = CreateAttribute(type);
            AttributeMap.Add(type, attribute);
        }
        //基础属性没变化
        if (attribute.BaseValue == value)
        {
            return;
        }
        _ = attribute.SetBase(value);
        IsNetDirty = true;

        OnAttributeUpdate(type, attribute);
    }

    /// <summary>
    /// 设置Real方式下属性基础值
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public void SetRealBaseValue(eAttributeType type, float value)
    {
        SetBaseValue(type, TableUtil.AttributeRealValueConvertToTable(value, type));
    }

    /// <summary>
    /// 获取属性基础值
    /// </summary>
    public int GetBaseValue(eAttributeType type)
    {
        if (AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            return attribute.BaseValue;
        }
        return 0;
    }

    /// <summary>
    /// 新增修改属性
    /// </summary>
    public IntAttributeModifier AddModifier(eAttributeType type, eModifierType modifierType, int value)
    {
        if (_isDestroy)
        {
            return null;
        }

        if (!AttributeMap.TryGetValue(type, out IntAttribute attribute))
        {
            attribute = CreateAttribute(type);
            AttributeMap.Add(type, attribute);
        }
        IntAttributeModifier modifier = attribute.AddModifier(type, modifierType, value);
        OnAttributeUpdate(type, attribute);
        return modifier;
    }

    /// <summary>
    /// 删除修改属性
    /// </summary>
    public void RemoveModifier(IntAttributeModifier modifier)
    {
        if (_isDestroy)
        {
            return;
        }

        if (!AttributeMap.TryGetValue(modifier.AttributeType, out IntAttribute attribute))
        {
            Log.Error($"EntityAttributeData RemoveModifier Not Find Attribute Type = {modifier.AttributeType}");
            return;
        }
        attribute.RemoveModifier(modifier);
        OnAttributeUpdate(modifier.AttributeType, attribute);
    }

    private IntAttribute CreateAttribute(eAttributeType type)
    {
        float coefficient = TableUtil.GetAttributeCoefficient(type);
        IntAttribute attribute = IntAttribute.Create(coefficient);
        return attribute;
    }

    public List<AttributeData> GetNetData()
    {
        if (IsNetDirty)
        {
            _netAttributeBaseDataList.Clear();
            foreach (KeyValuePair<eAttributeType, IntAttribute> item in AttributeMap)
            {
                _netAttributeBaseDataList.Add(GenerateOneNetAttribute(item.Key, item.Value));
            }
            IsNetDirty = false;
        }
        return _netAttributeBaseDataList;
    }

    /// <summary>
    /// 生成一个网络传输的属性数据 子类可以重写自己要传输的内容 默认是baseValue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    protected virtual AttributeData GenerateOneNetAttribute(eAttributeType type, IntAttribute attribute)
    {
        return new AttributeData()
        {
            Id = (int)type,
            Value = attribute.BaseValue,
        };
    }

    /// <summary>
    /// 某个属性更新
    /// </summary>
    /// <param name="type"></param>
    /// <param name="attribute"></param>
    protected virtual void OnAttributeUpdate(eAttributeType type, IntAttribute attribute) { }
}