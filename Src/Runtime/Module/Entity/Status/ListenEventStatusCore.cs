/** 
 * @Author XQ
 * @Date 2022-08-09 10:25:20
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/ListenEventStatusCore.cs
 */
using GameFramework.Fsm;
using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using System;

/// <summary>
/// 可以监听EntityEvent的状态
/// </summary>
public abstract class ListenEventStatusCore : EntityStatusCore
{
    public EntityEvent EntityEvent { get; private set; }

    /// <summary>
    /// 子类重写需要的事件功能类型 必须是EntityStatusEventFunctionBase的子类
    /// </summary>
    protected virtual Type[] EventFunctionTypes => null;

    private List<EntityStatusEventFunctionBase> _eventFunctions = null;

    protected override void OnInit(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnInit(fsm);

        Type[] types = EventFunctionTypes;
        if (types != null && types.Length > 0)
        {
            _eventFunctions = new();
            foreach (Type t in types)
            {
                if (ReferencePool.Acquire(t) is not EntityStatusEventFunctionBase func)
                {
                    Log.Error($"EventFunction {t.Name} is not EntityStatusEventFunctionBase");
                    continue;
                }

                func.Init(this, fsm, StatusCtrl);
                _eventFunctions.Add(func);
            }
        }
    }

    protected override void OnDestroy(IFsm<EntityStatusCtrl> fsm)
    {
        if (_eventFunctions != null && _eventFunctions.Count > 0)
        {
            foreach (EntityStatusEventFunctionBase func in _eventFunctions)
            {
                ReferencePool.Release(func);
            }
            _eventFunctions = null;
        }

        base.OnDestroy(fsm);
    }

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        EntityEvent = StatusCtrl.GetComponent<EntityEvent>();

        if (_eventFunctions != null && _eventFunctions.Count > 0)
        {
            foreach (EntityStatusEventFunctionBase func in _eventFunctions)
            {
                func.AddEvent(EntityEvent);
            }
        }

        AddEvent(EntityEvent);
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        RemoveEvent(EntityEvent);

        if (_eventFunctions != null && _eventFunctions.Count > 0)
        {
            foreach (EntityStatusEventFunctionBase func in _eventFunctions)
            {
                func.RemoveEvent(EntityEvent);
            }
        }

        EntityEvent = null;

        base.OnLeave(fsm, isShutdown);
    }

    /// <summary>
    /// 进入状态时的添加事件
    /// </summary>
    /// <param name="entityEvent"></param>
    protected virtual void AddEvent(EntityEvent entityEvent) { }

    /// <summary>
    /// 离开状态时移除事件
    /// </summary>
    /// <param name="entityEvent"></param>
    protected virtual void RemoveEvent(EntityEvent entityEvent) { }
}