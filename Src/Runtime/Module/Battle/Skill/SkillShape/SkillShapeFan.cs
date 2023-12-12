using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 扇形技能检测范围
/// </summary>
public class SkillShapeFan : SkillShapeBase
{
    private float _radius;
    private float _angle;
    private float _height;
    private Vector3 _forward;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center">扇形中心点</param>
    /// <param name="radius">扇形半径</param>
    /// <param name="angle">扇形角度，>=0&<=360</param>
    /// <param name="height">扇形柱高度</param>
    /// <param name="forward">扇形朝向</param>
    public void Init(Vector3 center, float radius, float angle, float height, Vector3 forward)
    {
        _radius = radius;
        _angle = Math.Clamp(angle, 0, 360);
        _height = height;
        _forward = forward;
        _forward.y = 0;//取xz面的投影，不考虑倾斜的情况，扇形柱固定垂直xz面
        Anchor = center;
        InitShape = true;
    }
    public override void DrawGizmos()
    {
        GizmosTools.DrawWireSemicircle(Anchor, _forward, _radius, _angle, Color.red);
    }

    protected override int CheckAll(int targetLayer, Collider[] noGCResult)
    {
        Vector3 pTop = Anchor + new Vector3(0, _height / 2, 0);
        Vector3 pBottom = Anchor - new Vector3(0, _height / 2, 0);
        //通过胶囊体检测360度的碰撞盒
        int res = Physics.OverlapCapsuleNonAlloc(pTop, pBottom, _radius, BuffOverlapColliderArray, targetLayer);

        BuffColliderList.Clear();
        for (int i = 0; i < res; i++)
        {
            Collider c = BuffOverlapColliderArray[i];
            //向量的y值都用Anchor.y,不然计算出来的角度是错误的
            Vector3 toTarget = new Vector3(c.transform.position.x, Anchor.y, c.transform.position.z) - Anchor;
            //通过角度计算碰撞盒是否在扇形范围内
            if (Vector3.Angle(toTarget, _forward) <= _angle / 2)
            {
                BuffColliderList.Add(c);
            }
        }

        if (BuffColliderList.Count > 0)
        {
            BuffColliderList.CopyTo(0, noGCResult, 0, BuffColliderList.Count);
        }
        return BuffColliderList.Count;
    }
}