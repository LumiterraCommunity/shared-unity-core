using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 家园采集资源core
/// </summary>
public abstract class HomeResourcesCore : EntityBaseComponent, ICollectResourceCore
{
    public ulong Id { get; private set; }

    public eResourceType ResourceType => eResourceType.HomeResource;

    public GameObject LogicRoot => RefEntity.EntityRoot;

    public Vector3 Position => RefEntity.Position;

    public bool IsDead { get; private set; }

    public eAction SupportAction { get; set; } = eAction.None;
    public ResourceDataCore Data { get; private set; }

    public int Lv => Data.DRHomeResources.Lv;

    private HomeSoilResourceRelation _addedSoilResourceRelation;//上次添加关系的组建 方式一个家园到另外家园瞬间的引用关系错误

    protected virtual void Awake()
    {
        IsDead = false;
    }

    protected virtual void Start()
    {
        if (HomeModuleCore.IsInited)//在家园里
        {
            if (Data != null)
            {
                _addedSoilResourceRelation = HomeModuleCore.SoilResourceRelation;
                _addedSoilResourceRelation.AddResourceOnSoil((long)Id, Data.SaveData.Id);
            }
            else
            {
                Log.Error($"家园采集资源 Data 组件 is null");
            }
        }
    }

    /// <summary>
    /// 初始化基础的ResourceData之后调用  用来第一次初始化基础的内容
    /// </summary>
    public void OnInitedResourceData()
    {
        Id = (ulong)RefEntity.BaseData.Id;
        Data = GetComponent<ResourceDataCore>();
        DRHomeResources dr = Data.DRHomeResources;
        SupportAction = TableUtil.ToHomeAction(dr.HomeAction);

        if ((SupportAction & eAction.Pick) != 0)//捡东西很特殊 没有进度但是需要选中显示进度icon 所以只能特殊处理一下 https://linear.app/project-linco/issue/LNCO-4361/砍树挖矿采草时，需要显示名字和提示等信息
        {
            GetComponent<HomeActionProgressData>().StartProgressAction(SupportAction, 1);//最大进度给1点 不要给0点 也加不了进度 所以给1没关系
        }
        else if ((PROGRESS_ACTION_MASK & SupportAction) != 0)
        {
            GetComponent<HomeActionProgressData>().StartProgressAction(SupportAction, dr.MaxActionValue);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_addedSoilResourceRelation)
        {
            _addedSoilResourceRelation.RemoveResourceOnSoil((long)Id);
            _addedSoilResourceRelation = null;
        }

        GetComponent<HomeActionProgressData>().EndProgressAction();
    }

    public bool CheckSupportAction(eAction action)
    {
        if (IsDead)
        {
            return false;
        }

        return (SupportAction & action) != 0;
    }

    public void ExecuteAction(eAction action, int toolCid, int skillId, long playerId, object actionData)
    {
        if (!CheckSupportAction(action))
        {
            Log.Error($"家园采集资源 action {action} not support,isDead:{IsDead}");
            return;
        }

        OnExecuteAction(action, skillId, playerId);

        IsDead = true;
        OnDeath();
    }

    protected virtual void OnExecuteAction(eAction action, int skillId, long playerId)
    {
    }

    public virtual void ExecuteProgress(eAction targetCurAction, int skillId, int deltaProgress, bool isCrit, bool isPreEffect, long playerId)
    {

    }

    /// <summary>
    /// 当采集资源死亡时
    /// </summary>
    protected virtual void OnDeath()
    {
    }
}