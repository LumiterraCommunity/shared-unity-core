using UnityEngine;
/// <summary>
/// 胶囊型范围技能
/// </summary>
public class SkillShapeCapsule : SkillShapeBase
{
    private Vector3 _p1;
    private Vector3 _p2;
    private float _radius;

    public override float Radius => _radius;

    /// <summary>
    /// 全参数构造函数
    /// </summary>
    /// <param name="p1">胶囊体端点1</param>
    /// <param name="p2">胶囊体端点2</param>
    /// <param name="radius">胶囊球形半径</param>
    /// <param name="anchor">伤害锚点,注意，该锚点是世界坐标系</param>
    public void Init(Vector3 p1, Vector3 p2, float radius, Vector3 anchor)
    {
        _p1 = p1;
        _p2 = p2;
        _radius = radius;
        Anchor = anchor;
        InitShape = true;
    }

    public void Init(Vector3 p1, Vector3 p2, float radius)
    {
        _p1 = p1;
        _p2 = p2;
        _radius = radius;
        Anchor = (_p1 + _p2) / 2;
        InitShape = true;
    }

    protected override int CheckAll(int targetLayer, Collider[] noGCResult)
    {
        return Physics.OverlapCapsuleNonAlloc(_p1, _p2, _radius, noGCResult, targetLayer);
    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_p1, _p2);
    }
}