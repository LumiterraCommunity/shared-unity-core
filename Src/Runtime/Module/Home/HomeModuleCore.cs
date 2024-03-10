using System;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 家园总模块core部分
/// </summary>
public abstract class HomeModuleCore : MonoBehaviour
{
    /// <summary>
    /// 家园模块根 获取模块再这个上面获取
    /// </summary>
    /// <value></value>
    public static GameObject Root { get; private set; }

    /// <summary>
    /// 家园模块是否初始化完成
    /// </summary>
    public static bool IsInited => Root != null;

    /// <summary>
    /// 土地管理
    /// </summary>
    /// <value></value>
    public static IHomeSoilMgr SoilMgr { get; private set; }

    /// <summary>
    /// 土地和上面的资源关系
    /// </summary>
    /// <value></value>
    public static HomeSoilResourceRelation SoilResourceRelation { get; private set; }

    /// <summary>
    /// 畜牧场景管理
    /// </summary>
    /// <value></value>
    public static HomeAnimalScene AnimalScene { get; private set; }
    /// <summary>
    /// 家园数据core
    /// </summary>
    /// <value></value>
    public static HomeDataCore HomeData { get; protected set; }
    public static HomeEntityFactoryCore<HomeEntityCore> EntityFactory { get; internal set; }

    protected virtual void Start()
    {
        if (Root != null)
        {
            Log.Error("HomeModuleCore已经初始化过了");
            Destroy(Root);
        }

        Root = gameObject;

        InitModule();

        StartInitLogic();
    }

    protected virtual void OnDestroy()
    {
        UnInitModule();

        if (Root == gameObject)
        {
            Root = null;
        }
    }

    protected virtual void InitModule()
    {
        SoilResourceRelation = gameObject.AddComponent<HomeSoilResourceRelation>();
        AnimalScene = gameObject.AddComponent<HomeAnimalScene>();
    }

    protected virtual void UnInitModule()
    {
        HomeData = null;
        SoilMgr = null;
        SoilResourceRelation = null;
    }

    /// <summary>
    /// 整个家园初始化完成后 开始的初始化逻辑
    /// </summary>
    protected virtual void StartInitLogic()
    {
        SoilMgr = AddHomeSoilMgr();

        _ = gameObject.AddComponent<InitHomeLogicCore>();
        _ = gameObject.AddComponent<AnimalInitLogicCore>();
    }

    /// <summary>
    /// 共享库中的土地管理器需要子类实现提供 子类将具体的土地管理器返回即可
    /// </summary>
    /// <returns></returns>
    protected abstract IHomeSoilMgr AddHomeSoilMgr();
}