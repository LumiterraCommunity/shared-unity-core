using Newtonsoft.Json;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 家园单块土地实体
/// </summary>
public abstract class HomeSoilCore : MonoBehaviour, ICollectResourceCore
{
    public ulong Id => SoilData.SaveData.Id;

    public eResourceType ResourceType => eResourceType.Soil;

    public Vector3 Position => transform.position;

    public GameObject LogicRoot => gameObject;


    public SoilEvent SoilEvent { get; set; }
    public SoilStatusCtrl StatusCtrl { get; private set; }
    public SoilData SoilData { get; private set; }

    public eAction SupportAction => GetCurStatus().SupportAction;

    public int GetActionLevel(eAction action)
    {
        Log.Error("HomeSoilCore.GetActionLevel() is not implemented");
        return 0;
    }

    /// <summary>
    /// 土地上的功能性实体 只有功能性种子成熟后且没有收割掉时才不为null
    /// </summary>
    /// <value></value>
    public HomeEntityCore FunctionEntity { get; private set; }

    protected virtual void Awake()
    {
        SoilEvent = gameObject.AddComponent<SoilEvent>();
        StatusCtrl = gameObject.AddComponent<SoilStatusCtrl>();
        SoilData = gameObject.AddComponent<SoilData>();

        InitStatus(StatusCtrl);
    }

    protected virtual void Start()
    {
        SoilEvent.OnFunctionSeedRipe += OnFunctionSeedRipe;
    }

    protected virtual void OnDestroy()
    {
        SoilEvent.OnFunctionSeedRipe -= OnFunctionSeedRipe;
    }

    /// <summary>
    /// 子类初始化具体的状态 前后端不一样
    /// </summary>
    /// <param name="statusCtrl"></param>
    protected abstract void InitStatus(SoilStatusCtrl statusCtrl);

    /// <summary>
    /// 获取当前状态
    /// </summary>
    /// <returns></returns>
    protected SoilStatusCore GetCurStatus()
    {
        return StatusCtrl.Fsm.CurrentState as SoilStatusCore;
    }

    public bool CheckSupportAction(eAction action)
    {
        return GetCurStatus().CheckSupportAction(action);
    }

    public void ExecuteAction(eAction action, int toolCid, long playerId, long entityId, int skillId, object actionData)
    {
        try
        {
            if (action == eAction.Sowing)
            {
                SoilEvent.MsgExecuteAction?.Invoke(eAction.Sowing, toolCid);
            }
            else if (action == eAction.Manure)
            {
                SoilEvent.MsgExecuteAction?.Invoke(eAction.Manure, toolCid);
            }
            else
            {
                SoilEvent.MsgExecuteAction?.Invoke(action, actionData);
            }
        }
        catch (System.Exception)
        {
            Log.Error($"土地执行动作失败 action:{action} toolCid:{toolCid} skillId:{skillId} actionData:{JsonConvert.SerializeObject(actionData)}");
        }

        if ((action & PROGRESS_ACTION_MASK) == 0)//非进度的动作 因为进度动作 在执行动作前会执行进度动作 已经触发过了
        {
            SoilEvent.OnBeHit?.Invoke(skillId);
        }
    }

    public void ExecuteProgress(eAction targetCurAction, long triggerEntityId, int skillId, int deltaProgress, bool isCrit, bool isPreEffect)
    {
        SoilEvent.OnBeHit?.Invoke(skillId);
    }

    private void OnFunctionSeedRipe(GameMessageCore.SeedFunctionType type)
    {
        GameObject entityRoot = GameObjectUtil.CreateGameObject("FunctionEntityRoot", LogicRoot.transform);
        HomeEntityCore entity = HomeModuleCore.HomeEntityMgr.AddEntity((long)SoilData.SaveData.Id, type, entityRoot);
        SetHomeEntity(entity);

        entity.Init(this);
    }

    /// <summary>
    /// 设置土地上的实体 建立关联关系
    /// </summary>
    /// <param name="entity"></param>
    private void SetHomeEntity(HomeEntityCore entity)
    {
        if (FunctionEntity != null && entity != null)
        {
            Log.Error("土地上已经有实体了");
            SetHomeEntity(null);
            return;
        }

        if (entity != null)//添加实体关系
        {
            FunctionEntity = entity;
            entity.EntityEvent.OnEntityRemoved += OnEntityRemoved;
        }
        else//移除实体关系
        {
            if (FunctionEntity != null)
            {
                FunctionEntity.EntityEvent.OnEntityRemoved -= OnEntityRemoved;
                FunctionEntity = null;
            }
        }
    }

    private void OnEntityRemoved()
    {
        SetHomeEntity(null);

        SoilEvent.OnFunctionSeedEntityRemoved?.Invoke();
    }
}