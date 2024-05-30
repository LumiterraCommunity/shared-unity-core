using System.Reflection;
using System;
/*
 * @Author: xiang huan
 * @Date: 2022-07-26 15:38:17
 * @Description: 共享库GFEntry引用
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/GFEntryCore.cs
 * 
 */
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

public static class GFEntryCore
{
    public static GFEntryType GFEntryType { get; set; } = GFEntryType.Client;
    private static List<object> s_GFEntryList = new();
    /// <summary>
    /// 获取数据表组件。
    /// </summary>
    private static IDataTableComponent s_DataTableComponent = null;
    /// <summary>
    /// IDataTableComponent比较特殊，由于DataTableComponent定义在Aot顶层模块中，编辑态无法获取到，所以需要通过反射的方式获取
    /// </summary>
    /// <value></value>
    public static IDataTableComponent DataTable
    {
        get
        {
            if (s_DataTableComponent == null)
            {
                try
                {
                    Type gameEntryType = Type.GetType("UnityGameFramework.Runtime.GameEntry, UnityGameFramework.Runtime");
                    if (gameEntryType == null)
                    {
                        Log.Error($"获取GameEntry类型失败，请检查GameEntry是否存在于UnityGameFramework.Runtime命名空间下");
                        return null;
                    }
                    Type dataTableComponentType = Type.GetType("UnityGameFramework.Runtime.DataTableComponent, UnityGameFramework.Runtime");
                    if (dataTableComponentType == null)
                    {
                        Log.Error($"获取DataTableComponent类型失败，请检查DataTableComponent是否存在于UnityGameFramework.Runtime命名空间下");
                        return null;
                    }
                    //获取参数类型是Type的GetComponent方法
                    MethodInfo method = gameEntryType.GetMethod("GetComponent", new Type[] { typeof(Type) });
                    //调用GetComponent方法
                    s_DataTableComponent = (IDataTableComponent)method.Invoke(null, new object[] { dataTableComponentType });
                }
                catch (Exception e)
                {
                    Log.Error($"Get DataTable failed, {e.Message}");
                    return null;
                }
            }

            return s_DataTableComponent;
        }
    }

    private static SkillEffectCoreFactory s_SkillEffectCoreFactory = null;
    public static SkillEffectCoreFactory SkillEffectFactory
    {
        get
        {
            if (s_SkillEffectCoreFactory == null)
            {
                s_SkillEffectCoreFactory = GetModule<SkillEffectCoreFactory>();
            }

            return s_SkillEffectCoreFactory;
        }
    }
    private static HomeResourcesAreaMgrCore s_homeResourcesAreaMgrCore = null;
    public static HomeResourcesAreaMgrCore HomeResourcesAreaMgr
    {
        get
        {
            if (s_homeResourcesAreaMgrCore == null)
            {
                s_homeResourcesAreaMgrCore = GetModule<HomeResourcesAreaMgrCore>();
            }

            return s_homeResourcesAreaMgrCore;
        }
    }

    private static SceneTriggerEventMgrCore s_sceneTriggerEventMgrCore = null;
    public static SceneTriggerEventMgrCore SceneTriggerEventMgr
    {
        get
        {
            if (s_sceneTriggerEventMgrCore == null)
            {
                s_sceneTriggerEventMgrCore = GetModule<SceneTriggerEventMgrCore>();
            }

            return s_sceneTriggerEventMgrCore;
        }
    }
    public static void AddModule(object module)
    {
        if (s_GFEntryList.IndexOf(module) != -1)
        {
            Log.Error($"GFEntry module is already exist, type {module.GetType().Name}.");
            return;
        }

        s_GFEntryList.Add(module);
    }

    public static void RemoveModule(object module)
    {
        bool remove = s_GFEntryList.Remove(module);
        if (!remove)
        {
            Log.Error($"GFEntry module is not exist, type {module.GetType().Name}.");
        }
    }
    /// <summary>
    /// 注意！！！频繁获取会有性能损耗，如果有频繁获取的需求，用成员变量缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static T GetModule<T>() where T : class
    {
        foreach (object item in s_GFEntryList)
        {
            if (item is T)
            {
                return item as T;
            }
        }

        Log.Error($"GFEntry module is not exist, type {typeof(T).Name}.");
        return null;
    }

    /// <summary>
    ///  模块是否存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static bool IsExistModule<T>() where T : class
    {
        foreach (object item in s_GFEntryList)
        {
            if (item is T)
            {
                return true;
            }
        }
        return false;
    }
    public static void SetGFEntryType(GFEntryType type)
    {
        GFEntryType = type;
    }
}
