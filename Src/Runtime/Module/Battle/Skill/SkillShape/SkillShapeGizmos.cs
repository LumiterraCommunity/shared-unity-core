/*
 * @Author: xiang huan
 * @Date: 2022-08-24 14:32:34
 * @Description: 技能范围绘制
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/Skill/SkillShape/SkillShapeGizmos.cs
 * 
 */
using UnityEngine;
public class SkillShapeGizmos : MonoBehaviour
{
    private SkillShapeBase _shape;
    public void StartDraw(int[] parameters, Vector3 pos, Vector3 dir)
    {
        if (_shape != null)
        {
            StopDraw();
        }

        _shape = SkillShapeFactory.CreateOneSkillShape(parameters, pos, dir);
    }

    public void StartDraw(int[] parameters, GameObject entity, Vector3 dir)
    {
        if (_shape != null)
        {
            StopDraw();
        }
        Vector3 startPos;
        if (entity.TryGetComponent(out RoleBaseDataCore roleData))
        {
            startPos = roleData.CenterPos;
        }
        else
        {
            startPos = entity.transform.position;
        }
        _shape = SkillShapeFactory.CreateOneSkillShape(parameters, startPos, dir);
    }
    public void StopDraw()
    {
        if (_shape != null)
        {
            SkillShapeBase.Release(_shape);
            _shape = null;
        }
    }
    private void OnDrawGizmos()
    {
        if (_shape != null)
        {
            _shape.DrawGizmos();
        }
    }

    private void OnDestroy()
    {
        StopDraw();
    }
}