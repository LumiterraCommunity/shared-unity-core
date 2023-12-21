using UnityEngine;
/// <summary>
/// 球形技能范围
/// </summary>
public class SkillShapeSphere : SkillShapeBase
{
    private Vector3 _center;
    private float _radius;

    public override float Radius => _radius;

    public void Init(Vector3 center, float radius)
    {
        _center = center;
        _radius = radius;
        Anchor = center;
        InitShape = true;
    }

    protected override int CheckAll(int targetLayer, Collider[] noGCResult)
    {
        return Physics.OverlapSphereNonAlloc(_center, _radius, noGCResult, targetLayer);
    }
    public override void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_center, _radius);
    }
}