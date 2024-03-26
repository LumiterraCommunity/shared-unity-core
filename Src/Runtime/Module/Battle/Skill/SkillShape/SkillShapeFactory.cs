/*
 * @Author: xiang huan
 * @Date: 2022-07-19 10:49:14
 * @Description: 技能碰撞检测生成工厂
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/Skill/SkillShape/SkillShapeFactory.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;


public class SkillShapeFactory
{
    public delegate SkillShapeBase CreateSkillShape(int[] parameters, Vector3 startPos, Vector3 dir);
    protected static Dictionary<BattleDefine.eSkillShapeId, CreateSkillShape> SkillShapeMap = new()
    {
        {BattleDefine.eSkillShapeId.SkillShapeBox, CreateSkillShapeBox},
        {BattleDefine.eSkillShapeId.SkillShapeSphere, CreateSkillShapeSphere},
        {BattleDefine.eSkillShapeId.SkillShapeCapsule, CreateSkillShapeCapsule},
        {BattleDefine.eSkillShapeId.SkillShapeFan, CreateSkillShapeFan},
    };

    /// <summary>
    /// 创建技能形状
    /// </summary>
    /// <returns></returns>
    public static SkillShapeBase CreateOneSkillShape(int[] parameters, Vector3 startPos, Vector3 dir)
    {
        if (parameters == null || parameters.Length < 1)
        {
            Log.Error($"CreateOneSkillShape Error parameters is Unknown");
            return null;
        }

        int shapeID = parameters[0];
        if (!SkillShapeMap.ContainsKey((BattleDefine.eSkillShapeId)shapeID))
        {
            Log.Error($"CreateOneSkillShape Error shapeID is Unknown shapeID = {shapeID}");
            return null;
        }
        SkillShapeBase shape = null;

        try
        {
            CreateSkillShape func = SkillShapeMap[(BattleDefine.eSkillShapeId)shapeID];
            shape = func.Invoke(parameters, startPos, dir);
        }
        catch (System.Exception)
        {
            Log.Error($"CreateOneSkillShape Create Error shapeID = {shapeID}");
        }

        return shape;
    }

    private static SkillShapeBase CreateSkillShapeBox(int[] parameters, Vector3 startPos, Vector3 dir)
    {
        if (parameters.Length < 4)
        {
            return null;
        }

        float xHalf = parameters[1] * MathUtilCore.CM2M / 2; //长
        float zHalf = parameters[2] * MathUtilCore.CM2M / 2; //宽
        float yHalf = parameters[3] * MathUtilCore.CM2M / 2; //高
        //前进距离
        float forwardDistance = 0;
        if (parameters.Length > 4)
        {
            forwardDistance = parameters[4] * MathUtilCore.CM2M;
        }
        Vector3 halfSize = new(xHalf, yHalf, zHalf);
        Vector3 moveDir = (dir.normalized * xHalf) + (dir.normalized * forwardDistance);
        Vector3 anchor = startPos;
        Vector3 centerPos = anchor + moveDir;
        Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        SkillShapeBox shape = SkillShapeBase.Create<SkillShapeBox>();
        shape.Init(centerPos, halfSize, rotation, anchor);
        return shape;
    }

    private static SkillShapeBase CreateSkillShapeSphere(int[] parameters, Vector3 startPos, Vector3 dir)
    {
        if (parameters.Length < 2)
        {
            return null;
        }

        float radius = parameters[1] * MathUtilCore.CM2M; //半径
                                                          //前进距离
        float forwardDistance = 0;
        if (parameters.Length > 2)
        {
            forwardDistance = parameters[2] * MathUtilCore.CM2M;
        }
        Vector3 centerPos = startPos + (dir.normalized * forwardDistance);
        SkillShapeSphere shape = SkillShapeBase.Create<SkillShapeSphere>();
        shape.Init(centerPos, radius);
        return shape;
    }
    private static SkillShapeBase CreateSkillShapeCapsule(int[] parameters, Vector3 startPos, Vector3 dir)
    {

        float height = parameters[1] * MathUtilCore.CM2M; //高度
        float radius = parameters[2] * MathUtilCore.CM2M; //半径
        float forwardDistance = 0;
        if (parameters.Length > 3)
        {
            forwardDistance = parameters[3] * MathUtilCore.CM2M;
        }
        Vector3 centerPos = startPos + (dir.normalized * forwardDistance);
        Vector3 pos1 = centerPos - (height / 2 * Vector3.up);
        Vector3 pos2 = centerPos + (height / 2 * Vector3.up);

        SkillShapeCapsule shape = SkillShapeBase.Create<SkillShapeCapsule>();
        shape.Init(pos1, pos2, radius, centerPos);
        return shape;
    }
    private static SkillShapeBase CreateSkillShapeFan(int[] parameters, Vector3 startPos, Vector3 dir)
    {
        float height = parameters[1] * MathUtilCore.CM2M; //高度
        float radius = parameters[2] * MathUtilCore.CM2M; //半径
        float angle = parameters[3] * MathUtilCore.CM2M; //角度
        float forwardDistance = 0;
        if (parameters.Length > 4)
        {
            forwardDistance = parameters[4] * MathUtilCore.CM2M;
        }
        Vector3 centerPos = startPos + (dir.normalized * forwardDistance);
        SkillShapeFan shape = SkillShapeBase.Create<SkillShapeFan>();
        shape.Init(centerPos, radius, angle, height, dir);
        return shape;
    }
}