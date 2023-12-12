/*
 * @Author: xiang huan
 * @Date: 2023-01-11 14:21:18
 * @Description: 实体整型属性
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/IntAttribute.cs
 * 
 */

using System;
using GameFramework;
using GameMessageCore;

public class IntAttribute : IReference
{
    /// <summary>
    /// 当前值
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// 乘以系数计算后的值
    /// </summary>
    public float RealValue { get; private set; }

    /// <summary>
    /// 乘法系数
    /// </summary>
    public float Coefficient { get; private set; }
    /// <summary>
    /// 基础值
    /// </summary>
    public int BaseValue { get; private set; }
    /// <summary>
    /// 增加值
    /// </summary>
    public int Add { get; private set; }
    /// <summary>
    /// 增加值百分比
    /// </summary>
    public int PctAdd { get; private set; }
    /// <summary>
    /// 最终增加值
    /// </summary>
    public int FinalAdd { get; private set; }
    /// <summary>
    /// 最终增加值百分比
    /// </summary>
    public int FinalPctAdd { get; private set; }
    private IntAttributeModifierCollector _addCollector { get; } = new IntAttributeModifierCollector();
    private IntAttributeModifierCollector _pctAddCollector { get; } = new IntAttributeModifierCollector();
    private IntAttributeModifierCollector _finalAddCollector { get; } = new IntAttributeModifierCollector();
    private IntAttributeModifierCollector _finalPctAddCollector { get; } = new IntAttributeModifierCollector();


    public static readonly float PERCENTAGE_FLAG = 1000f;


    public void Initialize(float coefficient)
    {
        Value = 0;
        BaseValue = Add = PctAdd = FinalAdd = FinalPctAdd = 0;
        RealValue = 0f;
        Coefficient = coefficient;
    }
    public int SetBase(int value)
    {
        BaseValue = value;
        Update();
        return BaseValue;
    }

    /// <summary>
    /// 添加属性修改
    /// </summary>
    public IntAttributeModifier AddModifier(eAttributeType type, eModifierType modifierType, int value)
    {
        IntAttributeModifier attribute = null;
        switch (modifierType)
        {
            case eModifierType.Add:
                attribute = _addCollector.AddModifier(type, modifierType, value);
                Add = _addCollector.TotalValue;
                break;
            case eModifierType.PctAdd:
                attribute = _pctAddCollector.AddModifier(type, modifierType, value);
                PctAdd = _pctAddCollector.TotalValue;
                break;
            case eModifierType.FinalAdd:
                attribute = _finalAddCollector.AddModifier(type, modifierType, value);
                FinalAdd = _finalAddCollector.TotalValue;
                break;
            case eModifierType.FinalPctAdd:
                attribute = _finalPctAddCollector.AddModifier(type, modifierType, value);
                FinalPctAdd = _finalPctAddCollector.TotalValue;
                break;
            default:
                break;
        }
        Update();
        return attribute;
    }

    /// <summary>
    /// 删除属性修改
    /// </summary>
    public void RemoveModifier(IntAttributeModifier modifier)
    {
        switch (modifier.Type)
        {
            case eModifierType.Add:
                _addCollector.RemoveModifier(modifier);
                Add = _addCollector.TotalValue;
                break;
            case eModifierType.PctAdd:
                _pctAddCollector.RemoveModifier(modifier);
                PctAdd = _pctAddCollector.TotalValue;
                break;
            case eModifierType.FinalAdd:
                _finalAddCollector.RemoveModifier(modifier);
                FinalAdd = _finalAddCollector.TotalValue;
                break;
            case eModifierType.FinalPctAdd:
                _finalPctAddCollector.RemoveModifier(modifier);
                FinalPctAdd = _finalPctAddCollector.TotalValue;
                break;
            default:
                break;
        }
        Update();
    }

    public void Update()
    {
        int value1 = BaseValue;
        float value2 = (value1 + Add) * (PERCENTAGE_FLAG + PctAdd) / PERCENTAGE_FLAG;
        float value3 = (value2 + FinalAdd) * (PERCENTAGE_FLAG + FinalPctAdd) / PERCENTAGE_FLAG;
        Value = (int)value3;
        RealValue = Value * Coefficient;
    }
    public virtual void Clear()
    {
        Initialize(1f);
        _addCollector.Clear();
        _pctAddCollector.Clear();
        _finalAddCollector.Clear();
        _finalPctAddCollector.Clear();
    }

    public static IntAttribute Create(float coefficient)
    {
        IntAttribute attribute = ReferencePool.Acquire<IntAttribute>();
        attribute.Initialize(coefficient);
        return attribute;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }
}