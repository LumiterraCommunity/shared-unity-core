/*
 * @Author: xiang huan
 * @Date: 2023-07-26 15:07:57
 * @Description: 技能效果方法基类
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFuncBase.cs
 * 
 */

using System;
using GameFramework;
public class SEFuncBase : IReference
{
    /// <summary>
    /// 技能效果
    /// </summary>
    protected SkillEffectBase SkillEffect;
    /// <summary>
    /// 宿主对象
    /// </summary>
    public EntityBase RefEntity { get; private set; }
    /// <summary>
    /// 方法类型
    /// </summary>
    public eSEFuncType Type { get; private set; }
    /// <summary>
    /// 方法参数
    /// </summary>
    public object FuncData { get; private set; }

    /// <summary>
    /// 是否更新
    /// </summary>
    public virtual bool IsUpdate => false;
    public virtual void Init()
    {
        OnAddEvent();
    }

    public void Update()
    {
        OnUpdate();
    }
    protected virtual void OnAddEvent()
    {

    }
    protected virtual void OnRemoveEvent()
    {

    }

    protected virtual void OnUpdate()
    {

    }
    public virtual void Clear()
    {
        SkillEffect = null;
        Type = eSEFuncType.None;
        FuncData = null;
    }

    public virtual void SetData(eSEFuncType type, SkillEffectBase skillEffect, object funcData = null)
    {
        SkillEffect = skillEffect;
        RefEntity = skillEffect.RefEntity;
        Type = type;
        FuncData = funcData;
    }

    public void Dispose()
    {
        OnRemoveEvent();
        ReferencePool.Release(this);
    }

    public static SEFuncBase Create(Type funcClass)
    {
        SEFuncBase func = ReferencePool.Acquire(funcClass) as SEFuncBase;
        return func;
    }
}