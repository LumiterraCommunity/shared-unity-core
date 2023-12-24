using UnityEngine;

/// <summary>
/// 自定义技能形状
/// </summary>
public class SkillShapeCustom : SkillShapeBase
{
    public override float Radius => 0;

    protected override int CheckAll(int targetLayer, Collider[] noGCResult)
    {
        throw new System.NotImplementedException();
    }
}