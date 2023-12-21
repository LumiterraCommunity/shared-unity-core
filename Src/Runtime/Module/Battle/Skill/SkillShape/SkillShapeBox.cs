using UnityEngine;

/// <summary>
/// 盒形范围技能
/// </summary>
public class SkillShapeBox : SkillShapeBase
{
    private Vector3 _halfSize;
    private Quaternion _rotation;
    private Vector3 _center;

    public override float Radius => _halfSize.x;//x是长


    /// <summary>
    /// 全参数初始化函数
    /// </summary>
    /// <param name="center">box中心点</param>
    /// <param name="halfSize">中心点到box的xyz面的距离，主要如果要表达变长为10的立方体，这是halfSize的值应该是new Vector3(10,10,10)</param>
    /// <param name="rotation">box的旋转状态</param>
    /// <param name="anchor">技能锚点,注意，该锚点是世界坐标系</param>
    public void Init(Vector3 center, Vector3 halfSize, Quaternion rotation, Vector3 anchor)
    {
        _center = center;
        _halfSize = halfSize;
        _rotation = rotation;
        Anchor = anchor;
        InitShape = true;
    }

    public void Init(Vector3 center, Vector3 halfSize, Quaternion rotation)
    {
        _center = center;
        _halfSize = halfSize;
        _rotation = rotation;
        Anchor = center;
        InitShape = true;
    }

    public void Init(Vector3 center, Vector3 halfSize)
    {
        _center = center;
        _halfSize = halfSize;
        _rotation = Quaternion.identity;
        Anchor = center;
        InitShape = true;
    }

    public void Init(Vector3 center, Vector3 halfSize, Vector3 anchor)
    {
        _center = center;
        _halfSize = halfSize;
        _rotation = Quaternion.identity;
        Anchor = anchor;
        InitShape = true;
    }

    protected override int CheckAll(int targetLayer, Collider[] noGCResult)
    {
        return Physics.OverlapBoxNonAlloc(_center, _halfSize, noGCResult, _rotation, targetLayer);
    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(_center, _rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawCube(Vector3.zero, _halfSize * 2);
        Gizmos.matrix = oldMatrix;
    }
}