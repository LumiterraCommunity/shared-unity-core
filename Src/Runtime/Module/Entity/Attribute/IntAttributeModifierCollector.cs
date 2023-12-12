/*
 * @Author: xiang huan
 * @Date: 2023-01-11 14:21:18
 * @Description: 属性修饰器列表
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/IntAttributeModifierCollector.cs
 * 
 */

using System.Collections.Generic;

public class IntAttributeModifierCollector
{
    public int TotalValue { get; private set; }
    private List<IntAttributeModifier> _modifiers { get; } = new();

    public IntAttributeModifier AddModifier(eAttributeType type, eModifierType modifierType, int value)
    {
        IntAttributeModifier modifier = IntAttributeModifier.Create(type, modifierType, value);
        _modifiers.Add(modifier);
        Update();
        return modifier;
    }

    public void RemoveModifier(IntAttributeModifier modifier)
    {
        _ = _modifiers.Remove(modifier);
        modifier.Dispose();
        Update();
    }

    public void Update()
    {
        TotalValue = 0;
        foreach (IntAttributeModifier item in _modifiers)
        {
            TotalValue += item.Value;
        }
    }

    public void Clear()
    {
        TotalValue = 0;
        for (int i = 0; i < _modifiers.Count; i++)
        {
            _modifiers[i].Dispose();
        }
        _modifiers.Clear();
    }
}