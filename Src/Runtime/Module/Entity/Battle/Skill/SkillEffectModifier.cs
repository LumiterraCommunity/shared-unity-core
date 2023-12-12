/*
 * @Author: xiang huan
 * @Date: 2023-03-07 19:11:21
 * @Description: 技能效果修改器
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Skill/SkillEffectModifier.cs
 * 
 */
using GameFramework;

public class SkillEffectModifier : IReference
{
    public eSkillEffectApplyType ApplyType { get; private set; }
    public eSkillEffectModifierType Type { get; private set; }

    public int[] EffectIDs { get; private set; } = new int[0];

    public int Value { get; private set; }
    public void Clear()
    {
        Type = eSkillEffectModifierType.Add;
        ApplyType = eSkillEffectApplyType.Init;
        EffectIDs = null;
        Value = 0;
    }
    public static SkillEffectModifier Create(eSkillEffectApplyType applyType, eSkillEffectModifierType type, params int[] values)
    {
        SkillEffectModifier modifier = ReferencePool.Acquire<SkillEffectModifier>();
        modifier.ApplyType = applyType;
        modifier.Type = type;
        modifier.EffectIDs = values;
        modifier.Value = type == eSkillEffectModifierType.Remove ? -1 : 1;
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