/*
 * @Author: xiang huan
 * @Date: 2023-01-11 14:21:18
 * @Description: 属性修饰器
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/IntAttributeModifier.cs
 * 
 */


using GameFramework;

public class IntAttributeModifier : IReference
{
    public eAttributeType AttributeType { get; private set; }
    public eModifierType Type { get; private set; }
    public int Value { get; private set; }
    public void Clear()
    {
        AttributeType = eAttributeType.Unknown;
        Type = eModifierType.Add;
        Value = 0;
    }
    public static IntAttributeModifier Create(eAttributeType attributeType, eModifierType type, int value)
    {
        IntAttributeModifier modifier = ReferencePool.Acquire<IntAttributeModifier>();
        modifier.AttributeType = attributeType;
        modifier.Type = type;
        modifier.Value = value;
        return modifier;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }
}