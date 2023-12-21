using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
/// <summary>
/// 技能形状基类，所有的技能形状都应该继承自这个类
/// </summary>
public abstract class SkillShapeBase : IReference
{
    /// <summary>
    /// 半径 也有点临时用 家园那边需要做客户端校验用 以后可以提供一个正式的boundingBox之类的
    /// </summary>
    /// <value></value>
    public abstract float Radius { get; }
    /// <summary>
    /// 子类实现shape的初始化时赋值为true
    /// </summary>
    protected bool InitShape = false;
    /// <summary>
    /// 子类需要按具体的形状来赋值
    /// </summary>
    protected Vector3 Anchor;

    //防止GC用 所有能碰到的元素
    protected readonly Collider[] BuffOverlapColliderArray = new Collider[BattleDefine.SKILL_CHECK_HIT_MAX_COLLIDER_NUM];
    //!!防止GC用 用之前记得清空 而且不要嵌套使用
    protected readonly List<Collider> BuffColliderList = new();

    /// <summary>
    /// 检测范围内收到攻击的碰撞体
    /// 可能返回null
    /// </summary>
    /// <param name="targetLayer">技能目标层</param>
    /// <param name="blockLayer">技能阻挡曾</param>
    /// <returns></returns>
    public Collider[] CheckHited(int targetLayer, int blockLayer)
    {
        if (!InitShape)
        {
            Log.Error("SkillShapeBase.CheckHited() - shape not init,return null");
            return null;
        }

        int res = CheckAll(targetLayer, BuffOverlapColliderArray);

        BuffColliderList.Clear();
        if (res > 0)
        {
            for (int i = 0; i < res; i++)
            {
                Collider collider = BuffOverlapColliderArray[i];
                if (blockLayer > 0)
                {
                    if (SkillUtil.CheckP2P(Anchor, collider.bounds.center, blockLayer))
                    {
                        BuffColliderList.Add(collider);
                    }
                }
                else
                {
                    BuffColliderList.Add(collider);
                }
            }
        }
        return BuffColliderList.ToArray();
    }

    /// <summary>
    /// 检测范围内收到攻击的碰撞体
    /// 这里没有传blockLayer，不考虑遮挡关系，范围内所有碰撞体都会被检测到
    /// </summary>
    /// <param name="targetLayer"></param>
    /// <param name="noGCResult">业务传入节省GC的数组</param>
    /// <returns>返回有效结果数量</returns>
    public int CheckHited(int targetLayer, Collider[] noGCResult)
    {
        if (!InitShape)
        {
            Log.Error("SkillShapeBase.CheckHited() - shape not init,return null");
            return 0;
        }

        return CheckAll(targetLayer, noGCResult);
    }

    /// <summary>
    /// 检测范围内targetLayer层的所有碰撞体
    /// </summary>
    /// <param name="targetLayer"></param>
    /// <param name="noGCResult">业务传入节省GC的数组</param>
    /// <returns>有效数量</returns>
    protected abstract int CheckAll(int targetLayer, Collider[] noGCResult);

    public void Clear()
    {
        InitShape = false;
    }

    /// <summary>
    /// 初始化技能形状
    /// 需要手动初始化技形状
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Create<T>() where T : SkillShapeBase, new()
    {
        T shape = ReferencePool.Acquire<T>();
        return shape;
    }

    /// <summary>
    /// 技能形状回池
    /// </summary>
    /// <param name="shape"></param>
    public static void Release(SkillShapeBase shape)
    {
        ReferencePool.Release(shape);
    }

    public virtual void DrawGizmos()
    {

    }
}